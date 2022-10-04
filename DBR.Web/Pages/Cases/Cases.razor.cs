using System.Linq.Expressions;
using System.Security.Claims;
using DBR.Core.Domain;
using DBR.Core.DTOs.Inputs;
using DBR.Core.DTOs.Outputs;
using DBR.Core.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace DBR.Web.Pages.Cases;

public partial class Cases
{
	[CascadingParameter] Task<AuthenticationState>? AuthenticationStateTask { get; set; }

	[Inject] IService<Case, CaseInputModel, CaseDTO> CaseService { get; set; } = default!;

	[Inject] IService<Member, Member, MemberDTO> MemberService { get; set; } = default!;

	readonly List<string> headerNames = new() { "Status", "Bilejers navn", "Bilejers tlf.nr.", "Nummerplade", "Stelnummer", "Model", "Mærke", "År", "" };
	readonly List<CaseDTO> cases = new();
	string? errorMessage;
	bool isLoading;
	Guid? memberWorkshopId;
	bool isReadyToDisplay = false;

	public override async Task SetParametersAsync(ParameterView parameters)
	{
		parameters.SetParameterProperties(this);

		if (parameters.TryGetValue(nameof(AuthenticationStateTask), out Task<AuthenticationState>? authStateTask))
		{
			if (authStateTask is not null)
			{
				ClaimsPrincipal user = (await authStateTask).User;

				if (user.Identity is not null && user.Identity.IsAuthenticated)
				{
					string memberIdString = user.FindFirstValue(ClaimTypes.NameIdentifier);

					if (!string.IsNullOrWhiteSpace(memberIdString))
					{
						ResponseDTO<MemberDTO> response = await MemberService.GetByIdAsync(Guid.Parse(memberIdString), selector: x => new Member { Id = x.Id, WorkshopId = x.WorkshopId }, cancellationToken: CancellationToken);

						if (response.Success)
						{
							memberWorkshopId = response.Content!.WorkshopId;
						}
						else
						{
							errorMessage = response.ErrorMessage;
						}
					}
				}
			}
		}

		await base.SetParametersAsync(ParameterView.Empty);
	}

	protected override async Task OnInitializedAsync()
	{
		await LoadCasesAsync();
	}

	async Task LoadCasesAsync()
	{
		isLoading = true;

		if (cases.Count is not 0)
		{
			cases.Clear();
		}

		Expression<Func<Case, bool>>? caseFilter = @case => @case.Workshop!.Id == memberWorkshopId;
		Expression<Func<Case, Case>>? caseSelector = @case => new Case
		{
			Id = @case.Id,
			Status = @case.Status,
			UpdatedDate = @case.UpdatedDate,
			CreatedDate = @case.CreatedDate,
			Customer = new Customer
			{
				Name = @case.Customer!.Name,
				PhoneNumber = @case.Customer!.PhoneNumber,
				Vehicle = new Vehicle
				{
					LicensePlate = @case.Customer!.Vehicle!.LicensePlate,
					VIN = @case.Customer!.Vehicle!.VIN,
					Model = @case.Customer!.Vehicle!.Model,
					Brand = @case.Customer!.Vehicle!.Brand,
					Year = @case.Customer!.Vehicle!.Year
				}
			}
		};
		static IIncludableQueryable<Case, object> CaseIncludes(IQueryable<Case> @case) => @case.Include(x => x.Customer!).ThenInclude(x => x.Vehicle!);
		static IOrderedQueryable<Case> CasesOrdered(IQueryable<Case> query) => query.OrderBy(x => x.Status).ThenByDescending(x => x.UpdatedDate).ThenByDescending(x => x.CreatedDate);

		try
		{
			IAsyncEnumerable<ResponseDTO<CaseDTO>> response = CaseService.GetAsync(filter: caseFilter, includeProperties: CaseIncludes, selector: caseSelector, orderBy: CasesOrdered, cancellationToken: CancellationToken);

			await foreach (ResponseDTO<CaseDTO> responseDTO in response)
			{
				if (responseDTO.Success)
				{
					cases.Add(responseDTO.Content!);
				}
				else
				{
					errorMessage = responseDTO.ErrorMessage;
				}
			}
		}
		finally
		{
			isReadyToDisplay = true;
			isLoading = false;
		}
	}
}