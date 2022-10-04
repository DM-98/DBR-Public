using System.Linq.Expressions;
using DBR.Core.Domain;
using DBR.Core.DTOs.Inputs;
using DBR.Core.DTOs.Outputs;
using DBR.Core.Interfaces;
using Microsoft.AspNetCore.Components;

namespace DBR.Web.Pages.Members;

public partial class CreateOrEditMember
{
	[Inject] IService<Member, Member, MemberDTO> MemberService { get; set; } = default!;

	[Inject] IAuthService AuthenticationService { get; set; } = default!;

	[Inject] IService<Workshop, WorkshopInputModel, WorkshopDTO> WorkshopService { get; set; } = default!;

	[Inject] NavigationManager NavigationManager { get; set; } = default!;

	[Parameter] public string CreateOrEdit { get; set; } = null!;

	[Parameter] public string? MemberId { get; set; }

	readonly List<WorkshopDTO> workshops = new();
	readonly RegisterInputModel memberInputModel = new();
	MemberDTO? memberToEdit;
	string? errorMessage;
	bool isInvalidPath;

	protected override async Task OnInitializedAsync()
	{
		if (CreateOrEdit is not "rediger" and not "opret")
		{
			errorMessage = "Hov, der er ingenting her.";
			isInvalidPath = true;

			return;
		}

		if (!string.IsNullOrWhiteSpace(MemberId))
		{
			ResponseDTO<MemberDTO> loadedMember = await MemberService.GetByIdAsync(Guid.Parse(MemberId), cancellationToken: CancellationToken);

			if (loadedMember.Success)
			{
				if (CreateOrEdit is "rediger")
				{
					memberToEdit = loadedMember.Content!;
				}
				else
				{
					memberInputModel.Name = loadedMember.Content!.UserName;
					memberInputModel.Email = loadedMember.Content!.Email;
					memberInputModel.PhoneNumber = loadedMember.Content!.PhoneNumber;
				}
			}
		}

		Expression<Func<Workshop, Workshop>>? workshopSelector = workshop => new Workshop
		{
			Id = workshop.Id,
			Name = workshop.Name
		};
		static IOrderedQueryable<Workshop> workshopsOrdered(IQueryable<Workshop> query) => query.OrderBy(x => x.Name);

		IAsyncEnumerable<ResponseDTO<WorkshopDTO>> response = WorkshopService.GetAsync(selector: workshopSelector, orderBy: workshopsOrdered, cancellationToken: CancellationToken);

		await foreach (ResponseDTO<WorkshopDTO> responseDTO in response)
		{
			if (!responseDTO.Success)
			{
				errorMessage = responseDTO.ErrorMessage;

				return;
			}

			workshops.Add(responseDTO.Content!);
			await InvokeAsync(StateHasChanged);
		}
	}

	async Task CreateOrEditMemberAsync()
	{
		if (CreateOrEdit is "rediger")
		{
			ResponseDTO<MemberDTO> updateMemberResponse = await MemberService.UpdateAsync(memberToEdit!, cancellationToken: CancellationToken);

			if (!updateMemberResponse.Success)
			{
				errorMessage = updateMemberResponse.ErrorMessage + " | " + updateMemberResponse.ExceptionMessage + " | " + updateMemberResponse.InnerExceptionMessage;

				return;
			}

			NavigationManager.NavigateTo($"medlemmer/detaljer/{MemberId}");
		}
		else
		{
			ResponseDTO<Member> registeredMember = await AuthenticationService.RegisterAsync(memberInputModel);

			if (!registeredMember.Success)
			{
				errorMessage = registeredMember.ErrorMessage;

				return;
			}

			NavigationManager.NavigateTo($"medlemmer/detaljer/{registeredMember.Content!.Id}");
		}
	}

	void NavigateBack()
	{
		NavigationManager.NavigateTo(string.IsNullOrWhiteSpace(MemberId) ? "medlemmer" : $"medlemmer/detaljer/{MemberId}");
	}
}