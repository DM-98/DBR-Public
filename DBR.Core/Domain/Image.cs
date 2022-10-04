using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBR.Core.Domain;

[Table("Images")]
public class Image : BaseEntity
{
	[StringLength(150)]
	public string ImageURL { get; set; } = null!;
}