using System.ComponentModel.DataAnnotations;

namespace DBR.Core.DTOs.Inputs;

public class CustomerInputModel
{
	[Required(ErrorMessage = "Dette felt skal udfyldes.")]
	[Display(Name = "Navn*")]
	[StringLength(50, ErrorMessage = "{0} kan højest indeholde {1} tegn.")]
	public string Name { get; set; } = null!;

	[Required(ErrorMessage = "Dette felt skal udfyldes.")]
	[Display(Name = "Mobilnummer*")]
	[StringLength(15, MinimumLength = 8, ErrorMessage = "{0} skal mindst være {2} tegn, og kan højest være {1} tegn.")]
	public string PhoneNumber { get; set; } = null!;

	[Display(Name = "Email")]
	[StringLength(50, ErrorMessage = "{0} kan højest indeholde {1} tegn.")]
	[EmailAddress(ErrorMessage = "{0} skal angives korrekt.")]
	public string? Email { get; set; }

	public Guid VehicleId { get; set; }
}