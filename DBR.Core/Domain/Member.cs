using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace DBR.Core.Domain;

[Table("Members")]
public class Member : IdentityUser<Guid>
{
	public Guid? WorkshopId { get; set; }

	[ForeignKey(nameof(WorkshopId))]
	public Workshop? Workshop { get; set; }

	public bool IsTermsAccepted { get; set; }

	[StringLength(150)]
	public string? RefreshToken { get; set; }

	public DateTime? RefreshTokenExpiryTime { get; set; }

	[DataType(DataType.DateTime)]
	public DateTime? CreatedDate { get; init; }

	[DataType(DataType.DateTime)]
	public DateTime? UpdatedDate { get; set; }

	public Member()
	{
		CreatedDate = DateTime.UtcNow;
	}
}