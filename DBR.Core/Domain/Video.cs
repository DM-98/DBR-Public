using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBR.Core.Domain;

[Table("Videos")]
public class Video : BaseEntity
{
	[StringLength(150)]
	public string VideoURL { get; set; } = null!;
}