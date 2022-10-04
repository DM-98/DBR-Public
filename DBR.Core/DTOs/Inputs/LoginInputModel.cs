using System.ComponentModel.DataAnnotations;

namespace DBR.Core.DTOs.Inputs;

public class LoginInputModel
{
	[Required(ErrorMessage = "{0} kan ikke være tom!")]
	[StringLength(50, ErrorMessage = "{0} kan højest indeholde {1} tegn.")]
	[EmailAddress(ErrorMessage = "{0} skal angives korrekt.")]
	[Display(Name = "Email")]
	public string Email { get; set; } = null!;

	[Required(ErrorMessage = "{0} kan ikke være tom!")]
	[StringLength(100, ErrorMessage = "{0} kan højest indeholde {1} tegn.")]
	[DataType(DataType.Password)]
	[Display(Name = "Adgangskode")]
	public string Password { get; set; } = null!;

	[Display(Name = "Forbliv logget ind")]
	public bool RememberMe { get; set; }
}