using System.ComponentModel.DataAnnotations;
using DBR.Core.Domain;

namespace DBR.Core.DTOs.Outputs;

public class VehicleDTO : BaseEntity
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