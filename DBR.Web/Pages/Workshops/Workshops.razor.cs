using System.Linq.Expressions;
using DBR.Core.Domain;
using DBR.Core.DTOs.Inputs;
using DBR.Core.DTOs.Outputs;
using DBR.Core.Interfaces;
using Microsoft.AspNetCore.Components;

namespace DBR.Web.Pages.Workshops;

public partial class Workshops
{
	[Inject] IService<Workshop, WorkshopInputModel, WorkshopDTO> WorkshopService { get; set; } = default!;

	readonly List<string> headerNames = new() { "Navn", "Telefonnummer", "Gadenavn og husnummer", "By", "Postnummer", "Antal medlemmer", "" };
	readonly List<WorkshopDTO> workshops = new();
	string? errorMessage;
	bool isLoading;
	bool isReadyToDisplay;

	protected override async Task OnInitializedAsync()
	{
		await LoadWorkshopsAsync();
	}

	async Task LoadWorkshopsAsync()
	{
		isLoading = true;

		if (workshops.Count is not 0)
		{
			workshops.Clear();
		}

		Expression<Func<Workshop, Workshop>>? workshopSelector = workshop => new Workshop
		{
			Id = workshop.Id,
			Name = workshop.Name,
			PhoneNumber = workshop.PhoneNumber,
			Address = new Address
			{
				Street = workshop.Address!.Street,
				City = workshop.Address!.City,
				PostCode = workshop.Address!.PostCode
			},
			Members = workshop.Members,
		};
		static IOrderedQueryable<Workshop> workshopsOrdered(IQueryable<Workshop> query) => query.OrderBy(x => x.Name).ThenByDescending(x => x.Address!.Street).ThenByDescending(x => x.Address!.City);

		try
		{
			IAsyncEnumerable<ResponseDTO<WorkshopDTO>> response = WorkshopService.GetAsync(selector: workshopSelector, orderBy: workshopsOrdered, cancellationToken: CancellationToken);

			await foreach (ResponseDTO<WorkshopDTO> responseDTO in response)
			{
				if (responseDTO.Success)
				{
					workshops.Add(responseDTO.Content!);
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