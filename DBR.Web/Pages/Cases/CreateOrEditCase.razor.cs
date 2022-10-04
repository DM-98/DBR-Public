using System.Linq.Expressions;
using System.Security.Claims;
using System.Text.Json;
using DBR.Core.Domain;
using DBR.Core.DTOs.Inputs;
using DBR.Core.DTOs.Outputs;
using DBR.Core.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace DBR.Web.Pages.Cases;

public partial class CreateOrEditCase
{
	[CascadingParameter] Task<AuthenticationState>? AuthenticationStateTask { get; set; }

	[Inject] IService<Customer, CustomerInputModel, CustomerDTO> CustomerService { get; set; } = default!;

	[Inject] IService<Vehicle, VehicleInputModel, VehicleDTO> VehicleService { get; set; } = default!;

	[Inject] IService<Case, CaseInputModel, CaseDTO> CaseService { get; set; } = default!;

	[Inject] IService<Member, Member, MemberDTO> MemberService { get; set; } = default!;

	[Inject] NavigationManager NavigationManager { get; set; } = default!;

	[Inject] IHttpClientFactory ClientFactory { get; set; } = default!;

	[Parameter] public string CreateOrEdit { get; set; } = null!;

	[Parameter] public string? CaseId { get; set; }

	readonly CaseInputModel caseInputModel = new();
	CaseDTO caseToEdit = new();
	string? errorMessage;
	string? licensePlateNotFound;
	bool isSearching;
	bool isInvalidPath;

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
						ResponseDTO<MemberDTO> loadedMember = await MemberService.GetByIdAsync(Guid.Parse(memberIdString), CancellationToken);

						if (loadedMember.Success && loadedMember.Content!.WorkshopId is not null)
						{
							caseInputModel.WorkshopId = (Guid)loadedMember.Content!.WorkshopId;
						}
					}
				}
			}
		}

		await base.SetParametersAsync(ParameterView.Empty);
	}

	public record MotorAPIResponse(string VIN, string Make, string Model, int Model_Year);

	protected override async Task OnInitializedAsync()
	{
		if (CreateOrEdit is not "rediger" and not "opret")
		{
			errorMessage = "Hov, der er ingenting her.";
			isInvalidPath = true;

			return;
		}

		if (!string.IsNullOrWhiteSpace(CaseId))
		{
			static IIncludableQueryable<Case, object> caseIncludes(IQueryable<Case> @case) => @case.Include(x => x.Customer!).ThenInclude(x => x.Vehicle!);
			Expression<Func<Case, Case>>? caseSelector = @case => new Case
			{
				Id = @case.Id,
				RowVersion = @case.RowVersion,
				Customer = new Customer
				{
					Id = @case.Customer!.Id,
					RowVersion = @case.Customer!.RowVersion,
					Name = @case.Customer!.Name,
					PhoneNumber = @case.Customer!.PhoneNumber,
					Vehicle = new Vehicle
					{
						Id = @case.Customer!.Vehicle!.Id,
						RowVersion = @case.Customer!.Vehicle!.RowVersion,
						LicensePlate = @case.Customer!.Vehicle!.LicensePlate,
						VIN = @case.Customer!.Vehicle!.VIN,
						Model = @case.Customer!.Vehicle!.Model,
						Brand = @case.Customer!.Vehicle!.Brand,
						Year = @case.Customer!.Vehicle!.Year,
						Kilometers = @case.Customer!.Vehicle!.Kilometers
					}
				}
			};

			ResponseDTO<CaseDTO> loadedCase = await CaseService.GetByIdAsync(Guid.Parse(CaseId), includeProperties: caseIncludes, selector: caseSelector, cancellationToken: CancellationToken);

			if (loadedCase.Success)
			{
				if (CreateOrEdit is "rediger")
				{
					caseToEdit = loadedCase.Content!;
				}
				else
				{
					caseInputModel.CustomerInputModel.Name = loadedCase.Content!.Customer!.Name;
					caseInputModel.CustomerInputModel.PhoneNumber = loadedCase.Content!.Customer!.PhoneNumber;

					caseInputModel.VehicleInputModel.LicensePlate = loadedCase.Content!.Customer!.Vehicle!.LicensePlate;
					caseInputModel.VehicleInputModel.VIN = loadedCase.Content!.Customer!.Vehicle!.VIN;
					caseInputModel.VehicleInputModel.Brand = loadedCase.Content!.Customer!.Vehicle!.Brand;
					caseInputModel.VehicleInputModel.Model = loadedCase.Content!.Customer!.Vehicle!.Model;
					caseInputModel.VehicleInputModel.Year = loadedCase.Content!.Customer!.Vehicle!.Year;
					caseInputModel.VehicleInputModel.Kilometers = loadedCase.Content!.Customer!.Vehicle!.Kilometers;
				}
			}
		}
	}

	async Task GetLicensePlateAsync(ChangeEventArgs eventArgs)
	{
		string? licensePlate = eventArgs.Value?.ToString();

		if (!isSearching && !string.IsNullOrWhiteSpace(licensePlate) && licensePlate.Length >= 4)
		{
			HttpClient httpClient = ClientFactory.CreateClient();
			httpClient.DefaultRequestHeaders.Add("x-auth-token", "150s25vdyiigu140d00j8mik44oel8tx");

			licensePlateNotFound = string.Empty;
			isSearching = true;

			try
			{
				string requestURI = $"https://v1.motorapi.dk/vehicles/{licensePlate}";
				MotorAPIResponse? response = await httpClient.GetFromJsonAsync<MotorAPIResponse?>(requestURI, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }, CancellationToken);

				if (response is not null)
				{
					caseInputModel.VehicleInputModel.Model = response.Model;
					caseInputModel.VehicleInputModel.Brand = response.Make;
					caseInputModel.VehicleInputModel.VIN = response.VIN;
					caseInputModel.VehicleInputModel.Year = Convert.ToInt16(response.Model_Year);
				}
			}
			catch (HttpRequestException)
			{
				licensePlateNotFound = "Kunne ikke finde biloplysninger ud fra nummerpladen. Prøv igen.";
			}
			finally
			{
				isSearching = false;
			}
		}
	}

	async Task CreateOrEditCaseAsync()
	{
		if (CreateOrEdit is "rediger")
		{
			ResponseDTO<CustomerDTO> updateCustomerResponse = await CustomerService.UpdateAsync(caseToEdit!.Customer!, CancellationToken);

			if (!updateCustomerResponse.Success)
			{
				errorMessage = updateCustomerResponse.ErrorMessage + " | " + updateCustomerResponse.ExceptionMessage + " | " + updateCustomerResponse.InnerExceptionMessage;

				return;
			}

			ResponseDTO<VehicleDTO> updateVehicleResponse = await VehicleService.UpdateAsync(caseToEdit!.Customer!.Vehicle!, CancellationToken);

			if (!updateVehicleResponse.Success)
			{
				errorMessage = updateVehicleResponse.ErrorMessage + " | " + updateVehicleResponse.ExceptionMessage + " | " + updateVehicleResponse.InnerExceptionMessage;

				return;
			}

			ResponseDTO<CaseDTO> updateCaseResponse = await CaseService.UpdateAsync(caseToEdit!, CancellationToken);

			if (!updateCaseResponse.Success)
			{
				errorMessage = updateCaseResponse.ErrorMessage + " | " + updateCaseResponse.ExceptionMessage + " | " + updateCaseResponse.InnerExceptionMessage;

				return;
			}

			NavigationManager.NavigateTo($"sager/detaljer/{CaseId}");
		}
		else
		{
			ResponseDTO<VehicleDTO> createdVehicle = await VehicleService.CreateAsync(caseInputModel.VehicleInputModel, CancellationToken);

			if (!createdVehicle.Success)
			{
				errorMessage = createdVehicle.ErrorMessage;

				return;
			}

			caseInputModel.CustomerInputModel.VehicleId = createdVehicle.Content!.Id;

			ResponseDTO<CustomerDTO> createdCustomer = await CustomerService.CreateAsync(caseInputModel.CustomerInputModel, CancellationToken);

			if (!createdCustomer.Success)
			{
				errorMessage = createdCustomer.ErrorMessage;

				return;
			}

			caseInputModel.CustomerId = createdCustomer.Content!.Id;

			ResponseDTO<CaseDTO> createdCase = await CaseService.CreateAsync(caseInputModel, CancellationToken);

			if (!createdCase.Success)
			{
				errorMessage = createdCase.ErrorMessage;

				return;
			}

			NavigationManager.NavigateTo($"sager/detaljer/{createdCase.Content!.Id}");
		}
	}

	void NavigateBack()
	{
		NavigationManager.NavigateTo(string.IsNullOrWhiteSpace(CaseId) ? "sager" : $"sager/detaljer/{CaseId}");
	}
}