using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBR.Core.Domain;

[Table("Addresses")]
public class Address : BaseEntity
{
	[StringLength(50)]
	public string Street { get; set; } = null!;

	[StringLength(25)]
	public string City { get; set; } = null!;

	[Required]
	public short? PostCode { get; set; }
}