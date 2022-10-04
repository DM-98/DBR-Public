using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBR.Core.Domain;

[Table("Workshops")]
public class Workshop : BaseEntity
{
	[StringLength(50)]
	public string Name { get; set; } = null!;

	public Guid AddressId { get; set; }

	[ForeignKey(nameof(AddressId))]
	public Address? Address { get; set; }

	[StringLength(15)]
	public string PhoneNumber { get; set; } = null!;

	public ICollection<Member>? Members { get; set; }

	public ICollection<Case>? Cases { get; set; }

	public ICollection<Specialization>? Specializations { get; set; }
}