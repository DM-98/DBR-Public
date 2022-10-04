using System.ComponentModel.DataAnnotations;
using DBR.Core.Domain;

namespace DBR.Core.DTOs.Outputs;

public class MemberDTO : BaseEntity
{
	public Guid? WorkshopId { get; set; }

	public WorkshopDTO? Workshop { get; set; }

	public bool IsTermsAccepted { get; set; }

	public string UserName { get; set; } = null!;

	public string Email { get; set; } = null!;

	public string? PhoneNumber { get; set; }

	[StringLength(150)]
	public string? RefreshToken { get; set; }

	public DateTime? RefreshTokenExpiryTime { get; set; }
}