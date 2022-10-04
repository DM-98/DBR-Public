using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace DBR.Web.Pages;

public partial class Index
{
	[CascadingParameter] Task<AuthenticationState>? AuthenticationStateTask { get; set; }

	[Inject] NavigationManager NavigationManager { get; set; } = default!;

	public override async Task SetParametersAsync(ParameterView parameters)
	{
		parameters.SetParameterProperties(this);

		if (parameters.TryGetValue(nameof(AuthenticationStateTask), out Task<AuthenticationState>? authStateTask))
		{
			if (authStateTask is not null)
			{
				ClaimsPrincipal user = (await authStateTask).User;

				if (user.Identity is not null)
				{
					NavigationManager.NavigateTo(user.Identity.IsAuthenticated ? "sager" : "log-ind");
				}
			}
		}

		await base.SetParametersAsync(ParameterView.Empty);
	}
}