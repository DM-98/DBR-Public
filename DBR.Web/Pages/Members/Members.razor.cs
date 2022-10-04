using System.Linq.Expressions;
using DBR.Core.Domain;
using DBR.Core.DTOs.Outputs;
using DBR.Core.Interfaces;
using Microsoft.AspNetCore.Components;

namespace DBR.Web.Pages.Members;

public partial class Members
{
	[Inject] IService<Member, Member, MemberDTO> MemberService { get; set; } = default!;

	readonly List<string> headerNames = new() { "Navn", "Email", "Telefonnummer", "Værksted", "Oprettelsesdato", "" };
	readonly List<MemberDTO> members = new();
	string? errorMessage;
	bool isLoading;
	bool isReadyToDisplay;

	protected override async Task OnInitializedAsync()
	{
		await LoadMembersAsync();
	}

	async Task LoadMembersAsync()
	{
		isLoading = true;

		if (members.Count is not 0)
		{
			members.Clear();
		}

		Expression<Func<Member, Member>>? memberSelector = member => new Member
		{
			Id = member.Id,
			UserName = member.UserName,
			Email = member.Email,
			PhoneNumber = member.PhoneNumber,
			CreatedDate = member.CreatedDate,
			Workshop = new Workshop
			{
				Name = member.Workshop!.Name
			}
		};
		static IOrderedQueryable<Member> membersOrdered(IQueryable<Member> query) => query.OrderBy(x => x.UserName).ThenBy(x => x.Email).ThenByDescending(x => x.CreatedDate);

		try
		{
			IAsyncEnumerable<ResponseDTO<MemberDTO>> response = MemberService.GetAsync(selector: memberSelector, orderBy: membersOrdered, cancellationToken: CancellationToken);

			await foreach (ResponseDTO<MemberDTO> responseDTO in response)
			{
				if (responseDTO.Success)
				{
					members.Add(responseDTO.Content!);
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