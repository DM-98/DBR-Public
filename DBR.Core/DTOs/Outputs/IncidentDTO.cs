using DBR.Core.Domain;
using DBR.Core.Enums;

namespace DBR.Core.DTOs.Outputs;

public class IncidentDTO : BaseEntity
{
	public CaseDTO? Case { get; set; }

	public string? ShortId { get; set; }

	public string Description { get; set; } = null!;

	public IncidentType Type { get; set; }

	public IEnumerable<AttachmentDTO>? Attachments { get; set; }
}