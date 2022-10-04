using DBR.Core.Domain;

namespace DBR.Core.DTOs.Outputs;

public class SpecializationDTO : BaseEntity
{
	public string Name { get; set; } = null!;

	public IEnumerable<WorkshopDTO>? Workshops { get; set; }
}