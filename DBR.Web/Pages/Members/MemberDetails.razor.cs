using System.Linq.Expressions;
using DBR.Core.Domain;
using DBR.Core.DTOs.Outputs;
using DBR.Core.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace DBR.Web.Pages.Members;

public partial class MemberDetails
{
	[Parameter] public string? MemberId { get; set; }

	[Inject] NavigationManager NavigationManager { get; set; } = default!;

	[Inject] IService<Member, Member, MemberDTO> MemberService { get; set; } = default!;

	MemberDTO? memberDetails;
	string? errorMessage;
	bool isLoading;

	protected override async Task OnInitializedAsync()
	{
		bool isMemberIdValid = Guid.TryParse(MemberId, out Guid memberId);

		if (!isMemberIdValid)
		{
			errorMessage = "Det valgte medlem er ugyldig - prøv igen.";

			return;
		}

		await GetMemberDetailsAsync(memberId);
	}

	async Task GetMemberDetailsAsync(Guid memberId)
	{
		isLoading = true;

		static IIncludableQueryable<Member, object> memberIncludes(IQueryable<Member> member) => member.Include(x => x.Workshop!).ThenInclude(x => x.Address!).Include(x => x.Workshop!).ThenInclude(x => x.Specializations!);
		Expression<Func<Member, Member>>? memberSelector = member => new Member
		{
			Id = member.Id,
			WorkshopId = member.WorkshopId,
			UserName = member.UserName,
			Email = member.Email,
			PhoneNumber = member.PhoneNumber,
			CreatedDate = member.CreatedDate,
			UpdatedDate = member.UpdatedDate,
			Workshop = new Workshop
			{
				Name = member.Workshop!.Name,
				Address = new Address
				{
					Street = member.Workshop!.Address!.Street,
					City = member.Workshop!.Address!.City,
					PostCode = member.Workshop!.Address!.PostCode
				},
				Specializations = member.Workshop!.Specializations!.Select(x => new Specialization
				{
					Name = x.Name
				}).ToList()
			}
		};

		ResponseDTO<MemberDTO> response = await MemberService.GetByIdAsync(memberId, includeProperties: memberIncludes, selector: memberSelector, cancellationToken: CancellationToken);

		if (response.Success)
		{
			memberDetails = response.Content!;
		}
		else
		{
			errorMessage = response.ErrorMessage;
		}

		isLoading = false;
	}

	void NavigateToEditMember()
	{
		NavigationManager.NavigateTo($"medlemmer/rediger/{MemberId}");
	}

	void NavigateBack()
	{
		NavigationManager.NavigateTo("medlemmer");
	}
}