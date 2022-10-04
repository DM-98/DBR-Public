using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBR.Core.Domain;

[Table("Vehicles")]
public class Vehicle : BaseEntity
{
	[StringLength(7)]
	public string LicensePlate { get; set; } = null!;

	[StringLength(25)]
	public string VIN { get; set; } = null!;

	[StringLength(50)]
	public string Model { get; set; } = null!;

	[StringLength(25)]
	public string Brand { get; set; } = null!;

	[Required]
	public short? Year { get; set; }

	public int? Kilometers { get; set; }
}