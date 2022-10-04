using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBR.Core.Domain;

[Table("Specializations")]
public class Specialization : BaseEntity
{
	[StringLength(25)]
	public string Name { get; set; } = null!;

	public ICollection<Workshop>? Workshops { get; set; }
}