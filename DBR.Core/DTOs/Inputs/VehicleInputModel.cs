using System.ComponentModel.DataAnnotations;

namespace DBR.Core.DTOs.Inputs;

public class VehicleInputModel
{
	[Required(ErrorMessage = "Dette felt skal udfyldes.")]
	[Display(Name = "Nummerplade*")]
	[StringLength(7, ErrorMessage = "{0} kan højest indeholde {1} tegn.")]
	public string LicensePlate { get; set; } = null!;

	[Required(ErrorMessage = "Dette felt skal udfyldes.")]
	[Display(Name = "Stelnummer")]
	[StringLength(25, ErrorMessage = "{0} kan højest indeholde {1} tegn.")]
	public string VIN { get; set; } = null!;

	[Required(ErrorMessage = "Dette felt skal udfyldes.")]
	[Display(Name = "Model")]
	[StringLength(50, ErrorMessage = "{0} kan højest indeholde {1} tegn.")]
	public string Model { get; set; } = null!;

	[Required(ErrorMessage = "Dette felt skal udfyldes.")]
	[Display(Name = "Mærke")]
	[StringLength(25, ErrorMessage = "{0} kan højest indeholde {1} tegn.")]
	public string Brand { get; set; } = null!;

	[Required(ErrorMessage = "Dette felt skal udfyldes.")]
	[Display(Name = "Årgang")]
	public short? Year { get; set; }

	[Display(Name = "Kilometertæller")]
	public int? Kilometers { get; set; }
}