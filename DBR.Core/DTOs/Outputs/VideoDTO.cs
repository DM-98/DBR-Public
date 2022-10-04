using DBR.Core.Domain;

namespace DBR.Core.DTOs.Outputs;

public class VideoDTO : BaseEntity
{
	public string VideoURL { get; set; } = null!;
}