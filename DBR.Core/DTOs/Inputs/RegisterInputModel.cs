using System.ComponentModel.DataAnnotations;

namespace DBR.Core.DTOs.Inputs;

public class RegisterInputModel
{
	[Required(ErrorMessage = "Dette felt skal udfyldes.")]
	[StringLength(50, ErrorMessage = "{0} kan højest indeholde {1} tegn.")]
	[EmailAddress(ErrorMessage = "{0} skal angives korrekt.")]
	[Display(Name = "Email*")]
	public string Email { get; set; } = null!;

	[Display(Name = "Telefonnummer")]
	public string? PhoneNumber { get; set; }

	[Required(ErrorMessage = "Dette felt skal udfyldes.")]
	[StringLength(50, MinimumLength = 3, ErrorMessage = "{0} skal være mellem {2}-{1} tegn.")]
	[Display(Name = "Navn*")]
	public string Name { get; set; } = null!;

	[Required(ErrorMessage = "Dette felt skal udfyldes.")]
	[StringLength(100, MinimumLength = 6, ErrorMessage = "{0} kan kun være mellem {2} og {1} tegn.")]
	[DataType(DataType.Password)]
	[Display(Name = "Adgangskode*")]
	public string Password { get; set; } = null!;

	[Required(ErrorMessage = "Dette felt skal udfyldes.")]
	[StringLength(100, MinimumLength = 6, ErrorMessage = "{0} kan kun være mellem {2} og {1} tegn.")]
	[DataType(DataType.Password)]
	[Compare(nameof(Password), ErrorMessage = "Adgangskode og bekræft adgangskode skal være ens.")]
	[Display(Name = "Bekræft adgangskode*")]
	public string ConfirmPassword { get; set; } = null!;

	[Required]
	[Range(typeof(bool), "true", "true", ErrorMessage = "For at registrere skal vedkommende acceptere servicevilkårene og privatlivspolitikken.")]
	[Display(Name = "Vedkommende accepterer Servicevilkårene og Privatlivspolitikken hos Dansk Bilbrancheråd*")]
	public bool IsTermsAccepted { get; set; }

	public Guid? WorkshopId { get; set; }
}