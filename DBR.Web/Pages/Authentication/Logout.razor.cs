using DBR.Web.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace DBR.Web.Pages.Authentication;

public partial class Logout
{
	[Inject] NavigationManager NavigationManager { get; set; } = default!;

	[Inject] ProtectedLocalStorage ProtectedLocalStorage { get; set; } = default!;

	[Inject] ProtectedSessionStorage ProtectedSessionStorage { get; set; } = default!;

	[Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

	protected override async Task OnInitializedAsync()
	{
		ProtectedBrowserStorageResult<string> rememberMe = await ProtectedLocalStorage.GetAsync<string>("authRememberMe");

		if (rememberMe.Success)
		{
			if (rememberMe.Value is "PLS")
			{
				await ProtectedLocalStorage.DeleteAsync("authAccessToken");
				await ProtectedLocalStorage.DeleteAsync("authRefreshToken");
			}
			else
			{
				await ProtectedSessionStorage.DeleteAsync("authAccessToken");
				await ProtectedSessionStorage.DeleteAsync("authRefreshToken");
			}
		}

		await ProtectedLocalStorage.DeleteAsync("authRememberMe");
		await ((AuthStateProvider)AuthenticationStateProvider).GetAuthenticationStateAsync();
		NavigationManager.NavigateTo("/");
	}
}