using System.Linq.Expressions;
using DBR.Core.Domain;
using DBR.Core.DTOs.Inputs;
using DBR.Core.DTOs.Outputs;
using DBR.Core.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace DBR.Web.Pages.Workshops;

public partial class WorkshopDetails
{
	[Parameter] public string? WorkshopId { get; set; }

	[Inject] IService<Workshop, WorkshopInputModel, WorkshopDTO> WorkshopService { get; set; } = default!;

	[Inject] NavigationManager NavigationManager { get; set; } = default!;

	WorkshopDTO? workshopDetails;
	string? errorMessage;
	bool isLoading;

	protected override async Task OnInitializedAsync()
	{
		bool isWorkshopIdValid = Guid.TryParse(WorkshopId, out Guid workshopId);

		if (!isWorkshopIdValid)
		{
			errorMessage = "Det valgte værksted er ugyldig - prøv igen.";
			isLoading = false;

			return;
		}

		await GetWorkshopDetailsAsync(workshopId);
	}

	async Task GetWorkshopDetailsAsync(Guid workshopId)
	{
		isLoading = true;

		static IIncludableQueryable<Workshop, object> workshopIncludes(IQueryable<Workshop> workshop) => workshop.Include(x => x.Address!).Include(x => x.Specializations!).Include(x => x.Members!);
		Expression<Func<Workshop, Workshop>>? workshopSelector = workshop => new Workshop
		{
			Id = workshop.Id,
			Name = workshop.Name,
			PhoneNumber = workshop.PhoneNumber,
			Specializations = workshop.Specializations!.Select(x => new Specialization
			{
				Name = x.Name
			}).ToList(),
			CreatedDate = workshop.CreatedDate,
			UpdatedDate = workshop.UpdatedDate,
			Address = new Address
			{
				Street = workshop.Address!.Street,
				City = workshop.Address!.City,
				PostCode = workshop.Address!.PostCode
			},
			Members = workshop.Members!.Select(x => new Member
			{
				Id = x.Id,
				UserName = x.UserName
			}).ToList()
		};

		ResponseDTO<WorkshopDTO> response = await WorkshopService.GetByIdAsync(workshopId, includeProperties: workshopIncludes, selector: workshopSelector, cancellationToken: CancellationToken);

		if (response.Success)
		{
			workshopDetails = response.Content!;
		}
		else
		{
			errorMessage = response.ErrorMessage;
		}

		isLoading = false;
	}

	void NavigateToEditWorkshop()
	{
		NavigationManager.NavigateTo($"værksteder/rediger/{WorkshopId}");
	}

	void NavigateBack()
	{
		NavigationManager.NavigateTo("værksteder");
	}
}