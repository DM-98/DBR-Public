using System.Security.Claims;
using DBR.Core.DTOs.Outputs;
using DBR.Core.Helpers;
using DBR.Core.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace DBR.Web.Shared;

public class AuthStateProvider : AuthenticationStateProvider
{
	private readonly ProtectedLocalStorage protectedLocalStorage;
	private readonly ProtectedSessionStorage protectedSessionStorage;
	private readonly IAuthService authService;

	public AuthStateProvider(ProtectedLocalStorage protectedLocalStorage, ProtectedSessionStorage protectedSessionStorage, IAuthService authService)
	{
		this.protectedLocalStorage = protectedLocalStorage;
		this.protectedSessionStorage = protectedSessionStorage;
		this.authService = authService;
	}

	public override async Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		ClaimsIdentity? claimsIdentity = new();

		ProtectedBrowserStorageResult<string> rememberMe = await protectedLocalStorage.GetAsync<string>("authRememberMe");
		ProtectedBrowserStorageResult<string> accessToken = default;
		ProtectedBrowserStorageResult<string> refreshToken = default;

		if (rememberMe.Success)
		{
			if (rememberMe.Value is "PLS")
			{
				accessToken = await protectedLocalStorage.GetAsync<string>("authAccessToken");
				refreshToken = await protectedLocalStorage.GetAsync<string>("authRefreshToken");
			}

			if (rememberMe.Value is "PSS")
			{
				accessToken = await protectedSessionStorage.GetAsync<string>("authAccessToken");
				refreshToken = await protectedSessionStorage.GetAsync<string>("authRefreshToken");
			}
		}

		if (accessToken.Success && refreshToken.Success)
		{
			claimsIdentity = new ClaimsIdentity(AuthHelper.ParseClaimsFromJWT(accessToken.Value!), "jwtAuthType");
			DateTimeOffset accessTokenExpiry = DateTimeOffset.FromUnixTimeSeconds(long.Parse(claimsIdentity.FindFirst("exp")!.Value));

			if (accessTokenExpiry <= DateTime.UtcNow.AddMinutes(1))
			{
				string? newAccessToken = await RefreshTokenAsync(Tuple.Create(accessToken.Value!, refreshToken.Value!));
				claimsIdentity = !string.IsNullOrWhiteSpace(newAccessToken) ? new ClaimsIdentity(AuthHelper.ParseClaimsFromJWT(newAccessToken), "jwtAuthType") : new();
			}
		}

		ClaimsPrincipal claimsPrincipal = new(claimsIdentity);
		AuthenticationState authenticationState = new(claimsPrincipal);

		NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));

		return authenticationState;
	}

	public async Task<string?> RefreshTokenAsync(Tuple<string, string> accessRefreshTokens)
	{
		ResponseDTO<Tuple<string, string>> result = await authService.RefreshTokenAsync(accessRefreshTokens);
		ProtectedBrowserStorageResult<string> rememberMe = await protectedLocalStorage.GetAsync<string>("authRememberMe");

		if (result.Success)
		{
			if (rememberMe.Value is "PLS")
			{
				await protectedLocalStorage.SetAsync("authAccessToken", result.Content!.Item1);
				await protectedLocalStorage.SetAsync("authRefreshToken", result.Content.Item2);
			}

			if (rememberMe.Value is "PSS")
			{
				await protectedSessionStorage.SetAsync("authAccessToken", result.Content!.Item1);
				await protectedSessionStorage.SetAsync("authRefreshToken", result.Content.Item2);
			}
		}
		else
		{
			if (rememberMe.Value is "PLS")
			{
				await protectedLocalStorage.DeleteAsync("authAccessToken");
				await protectedLocalStorage.DeleteAsync("authRefreshToken");
			}

			if (rememberMe.Value is "PSS")
			{
				await protectedSessionStorage.DeleteAsync("authAccessToken");
				await protectedSessionStorage.DeleteAsync("authRefreshToken");
			}

			await protectedLocalStorage.DeleteAsync("authRememberMe");
		}

		return result.Content?.Item1;
	}
}