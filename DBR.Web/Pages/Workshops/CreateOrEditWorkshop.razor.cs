using System.Linq.Expressions;
using DBR.Core.Domain;
using DBR.Core.DTOs.Inputs;
using DBR.Core.DTOs.Outputs;
using DBR.Core.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace DBR.Web.Pages.Workshops;

public partial class CreateOrEditWorkshop
{
	[Inject] IService<Workshop, WorkshopInputModel, WorkshopDTO> WorkshopService { get; set; } = default!;

	[Inject] IService<Address, AddressInputModel, AddressDTO> AddressService { get; set; } = default!;

	[Inject] NavigationManager NavigationManager { get; set; } = default!;

	[Parameter] public string CreateOrEdit { get; set; } = null!;

	[Parameter] public string? WorkshopId { get; set; }

	readonly WorkshopInputModel workshopInputModel = new();
	WorkshopDTO? workshopToEdit;
	string? errorMessage;
	bool isInvalidPath;

	protected override async Task OnInitializedAsync()
	{
		if (CreateOrEdit is not "rediger" and not "opret")
		{
			errorMessage = "Hov, der er ingenting at se her.";
			isInvalidPath = true;

			return;
		}

		if (!string.IsNullOrWhiteSpace(WorkshopId))
		{
			static IIncludableQueryable<Workshop, object> workshopIncludes(IQueryable<Workshop> workshop) => workshop.Include(x => x.Address!).Include(x => x.Members!);
			Expression<Func<Workshop, Workshop>>? workshopSelector = workshop => new Workshop
			{
				Id = workshop.Id,
				RowVersion = workshop.RowVersion,
				Name = workshop.Name,
				PhoneNumber = workshop.PhoneNumber,
				Address = new Address
				{
					Id = workshop.Address!.Id,
					RowVersion = workshop.Address!.RowVersion,
					Street = workshop.Address!.Street,
					City = workshop.Address!.City,
					PostCode = workshop.Address!.PostCode
				}
			};

			ResponseDTO<WorkshopDTO> loadedWorkshop = await WorkshopService.GetByIdAsync(Guid.Parse(WorkshopId), includeProperties: workshopIncludes, selector: workshopSelector, cancellationToken: CancellationToken);

			if (loadedWorkshop.Success)
			{
				if (CreateOrEdit is "rediger")
				{
					workshopToEdit = loadedWorkshop.Content!;
				}
				else
				{
					workshopInputModel.Name = loadedWorkshop.Content!.Name;
					workshopInputModel.PhoneNumber = loadedWorkshop.Content!.PhoneNumber;

					workshopInputModel.AddressInputModel.Street = loadedWorkshop.Content!.Address!.Street;
					workshopInputModel.AddressInputModel.City = loadedWorkshop.Content!.Address.City;
					workshopInputModel.AddressInputModel.PostCode = loadedWorkshop.Content!.Address.PostCode;
				}
			}
			else
			{
				errorMessage = loadedWorkshop.ErrorMessage + " | " + loadedWorkshop.ExceptionMessage + " | " + loadedWorkshop.InnerExceptionMessage;
			}
		}
	}

	async Task CreateOrEditWorkshopAsync()
	{
		if (CreateOrEdit is "rediger")
		{
			ResponseDTO<AddressDTO> updateAddressResponse = await AddressService.UpdateAsync(workshopToEdit!.Address!, CancellationToken);

			if (!updateAddressResponse.Success)
			{
				errorMessage = updateAddressResponse.ErrorMessage + " | " + updateAddressResponse.ExceptionMessage + " | " + updateAddressResponse.InnerExceptionMessage;

				return;
			}

			ResponseDTO<WorkshopDTO> updateWorkshopResponse = await WorkshopService.UpdateAsync(workshopToEdit!, CancellationToken);

			if (!updateWorkshopResponse.Success)
			{
				errorMessage = updateWorkshopResponse.ErrorMessage + " | " + updateWorkshopResponse.ExceptionMessage + " | " + updateWorkshopResponse.InnerExceptionMessage;

				return;
			}

			NavigationManager.NavigateTo($"værksteder/detaljer/{WorkshopId}");
		}
		else
		{
			ResponseDTO<AddressDTO> createdAddress = await AddressService.CreateAsync(workshopInputModel.AddressInputModel, CancellationToken);

			if (!createdAddress.Success)
			{
				errorMessage = createdAddress.ErrorMessage;

				return;
			}

			workshopInputModel.AddressId = createdAddress.Content!.Id;

			ResponseDTO<WorkshopDTO> createdWorkshop = await WorkshopService.CreateAsync(workshopInputModel, CancellationToken);

			if (!createdWorkshop.Success)
			{
				errorMessage = createdWorkshop.ErrorMessage;

				return;
			}

			NavigationManager.NavigateTo($"værksteder/detaljer/{createdWorkshop.Content!.Id}");
		}
	}

	void NavigateBack()
	{
		NavigationManager.NavigateTo(string.IsNullOrWhiteSpace(WorkshopId) ? "værksteder" : $"værksteder/detaljer/{WorkshopId}");
	}
}