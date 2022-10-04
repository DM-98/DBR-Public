using System.ComponentModel.DataAnnotations;

namespace DBR.Core.Domain;

public abstract class BaseEntity
{
	[Key]
	public Guid Id { get; init; }

	[Timestamp]
	public byte[] RowVersion { get; set; } = null!;

	public DateTime CreatedDate { get; init; }

	public DateTime? UpdatedDate { get; set; }

	public BaseEntity()
	{
		CreatedDate = DateTime.UtcNow;
	}
}