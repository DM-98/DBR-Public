using System.Linq.Expressions;
using System.Security.Claims;
using DBR.Core.Domain;
using DBR.Core.DTOs.Inputs;
using DBR.Core.DTOs.Outputs;
using DBR.Core.Enums;
using DBR.Core.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace DBR.Web.Pages.Cases;

public partial class CaseDetails
{
	[Parameter] public string? CaseId { get; set; }

	[CascadingParameter] Task<AuthenticationState>? AuthenticationStateTask { get; set; }

	[Inject] IService<Case, CaseInputModel, CaseDTO> CaseService { get; set; } = default!;

	[Inject] NavigationManager NavigationManager { get; set; } = default!;

	CaseDTO? caseDetails;
	string? errorMessage;
	bool isLoading;
	bool isAuthenticated;

	public override async Task SetParametersAsync(ParameterView parameters)
	{
		parameters.SetParameterProperties(this);

		if (parameters.TryGetValue(nameof(AuthenticationStateTask), out Task<AuthenticationState>? authStateTask))
		{
			if (authStateTask is not null)
			{
				ClaimsPrincipal claimsPrincipal = (await authStateTask).User;

				if (claimsPrincipal.Identity is not null && claimsPrincipal.Identity.IsAuthenticated)
				{
					isAuthenticated = true;
				}
			}
		}

		await base.SetParametersAsync(ParameterView.Empty);
	}

	protected override async Task OnInitializedAsync()
	{
		bool isCaseIdValid = Guid.TryParse(CaseId, out Guid caseId);

		if (!isCaseIdValid)
		{
			errorMessage = "Den valgte sag er ugyldig - prøv igen.";
			isLoading = false;

			return;
		}

		await GetCaseDetailsAsync(caseId);
	}

	async Task GetCaseDetailsAsync(Guid caseId)
	{
		isLoading = true;

		static IIncludableQueryable<Case, object> caseIncludes(IQueryable<Case> @case) => @case.Include(x => x.Customer!).ThenInclude(x => x.Vehicle!);
		Expression<Func<Case, Case>>? caseSelector = @case => new Case
		{
			Id = @case.Id,
			RowVersion = @case.RowVersion,
			Status = @case.Status,
			UpdatedDate = @case.UpdatedDate,
			CreatedDate = @case.CreatedDate,
			Incidents = @case.Incidents!.Select(x => new Incident
			{
				Type = x.Type,
				Description = x.Description,
				CreatedDate = x.CreatedDate,
				UpdatedDate = x.UpdatedDate,
				Attachments = x.Attachments!.Select(x => new Attachment
				{
					Id = x.Id,
					Type = x.Type,
					Invoice = new Invoice
					{
						InvoiceURL = x.Invoice!.InvoiceURL
					},
					Image = new Image
					{
						ImageURL = x.Image!.ImageURL
					},
					Video = new Video
					{
						VideoURL = x.Video!.VideoURL
					}
				}).ToList()
			}).ToList(),
			Customer = new Customer
			{
				Id = @case.Customer!.Id,
				Name = @case.Customer!.Name,
				PhoneNumber = @case.Customer!.PhoneNumber,
				Vehicle = new Vehicle
				{
					LicensePlate = @case.Customer!.Vehicle!.LicensePlate,
					VIN = @case.Customer!.Vehicle!.VIN,
					Model = @case.Customer!.Vehicle!.Model,
					Brand = @case.Customer!.Vehicle!.Brand,
					Year = @case.Customer!.Vehicle!.Year,
					Kilometers = @case.Customer!.Vehicle!.Kilometers
				}
			}
		};

		ResponseDTO<CaseDTO> response = await CaseService.GetByIdAsync(caseId, includeProperties: caseIncludes, selector: caseSelector, cancellationToken: CancellationToken);

		if (response.Success)
		{
			caseDetails = response.Content!;
		}
		else
		{
			errorMessage = response.ErrorMessage;
		}

		isLoading = false;
	}

	async Task ToggleCaseStatusAsync()
	{
		caseDetails!.Status = caseDetails.Status is CaseStatus.Active ? CaseStatus.Closed : CaseStatus.Active;

		ResponseDTO<CaseDTO> response = await CaseService.UpdateAsync(caseDetails, cancellationToken: CancellationToken);

		if (response.Success)
		{
			await GetCaseDetailsAsync(Guid.Parse(CaseId!));
		}
		else
		{
			errorMessage = response.ErrorMessage + " | " + response.ExceptionMessage + " | " + response.InnerExceptionMessage;
		}
	}

	void NavigateToCreateIncident()
	{
		NavigationManager.NavigateTo($"sager/detaljer/{CaseId}/opret-hændelse");
	}

	void NavigateToEditCase()
	{
		NavigationManager.NavigateTo($"sager/rediger/{CaseId}");
	}

	void NavigateToCreateCase()
	{
		NavigationManager.NavigateTo($"sager/opret/{CaseId}");
	}

	void NavigateBack()
	{
		NavigationManager.NavigateTo("sager");
	}
}