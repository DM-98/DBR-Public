using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBR.Core.Domain;

[Table("Customers")]
public class Customer : BaseEntity
{
	[StringLength(50)]
	public string Name { get; set; } = null!;

	[StringLength(15)]
	public string PhoneNumber { get; set; } = null!;

	[StringLength(50)]
	public string? Email { get; set; }

	public Guid VehicleId { get; set; }

	[ForeignKey(nameof(VehicleId))]
	public Vehicle? Vehicle { get; set; }
}