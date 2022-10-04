using DBR.Core.DTOs.Inputs;
using DBR.Core.DTOs.Outputs;
using DBR.Core.Interfaces;
using DBR.Web.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace DBR.Web.Pages.Authentication;

public partial class Login
{
	[Parameter]
	[SupplyParameterFromQuery]
	public string? ReturnUrl { get; set; }

	[Inject] IAuthService AuthService { get; set; } = default!;

	[Inject] NavigationManager NavigationManager { get; set; } = default!;

	[Inject] ProtectedLocalStorage ProtectedLocalStorage { get; set; } = default!;

	[Inject] ProtectedSessionStorage ProtectedSessionStorage { get; set; } = default!;

	[Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

	readonly LoginInputModel loginInputModel = new();
	bool isLoading;
	string? errorMessage;

	async Task LoginAsync()
	{
		isLoading = true;

		ResponseDTO<Tuple<string, string>> loginResponse = await AuthService.LoginAsync(loginInputModel);

		if (!loginResponse.Success)
		{
			errorMessage = loginResponse.ErrorMessage;
			isLoading = false;

			return;
		}

		if (loginInputModel.RememberMe)
		{
			await ProtectedLocalStorage.SetAsync("authAccessToken", loginResponse.Content!.Item1);
			await ProtectedLocalStorage.SetAsync("authRefreshToken", loginResponse.Content!.Item2);
			await ProtectedLocalStorage.SetAsync("authRememberMe", "PLS");
		}
		else
		{
			await ProtectedSessionStorage.SetAsync("authAccessToken", loginResponse.Content!.Item1);
			await ProtectedSessionStorage.SetAsync("authRefreshToken", loginResponse.Content!.Item2);
			await ProtectedLocalStorage.SetAsync("authRememberMe", "PSS");
		}

		await ((AuthStateProvider)AuthenticationStateProvider).GetAuthenticationStateAsync();

		NavigationManager.NavigateTo(string.IsNullOrWhiteSpace(ReturnUrl) ? "sager" : ReturnUrl);

		isLoading = false;
	}
}