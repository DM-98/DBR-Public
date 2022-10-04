using DBR.Core.Domain;

namespace DBR.Core.DTOs.Outputs;

public class ImageDTO : BaseEntity
{
	public string ImageURL { get; set; } = null!;
}