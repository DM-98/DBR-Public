using System.ComponentModel.DataAnnotations;

namespace DBR.Core.DTOs.Inputs;

public class WorkshopInputModel
{
	[Required(ErrorMessage = "Dette felt skal udfyldes.")]
	[StringLength(50, ErrorMessage = "{0} kan højest indeholde {1} tegn.")]
	[Display(Name = "Navn*")]
	public string Name { get; set; } = null!;

	[Required(ErrorMessage = "Dette felt skal udfyldes.")]
	[StringLength(15, MinimumLength = 8, ErrorMessage = "{0} skal mindst være {1} tegn, og kan højest være {2} tegn.")]
	[Display(Name = "Telefonnummer*")]
	public string PhoneNumber { get; set; } = null!;

	[ValidateComplexType]
	public AddressInputModel AddressInputModel { get; set; }

	public Guid AddressId { get; set; }

	public WorkshopInputModel()
	{
		AddressInputModel = new();
	}
}