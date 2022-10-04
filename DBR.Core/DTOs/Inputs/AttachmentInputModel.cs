using DBR.Core.Enums;

namespace DBR.Core.DTOs.Inputs;

public class AttachmentInputModel
{
	public Guid IncidentId { get; set; }

	public Guid? VideoId { get; set; }

	public Guid? ImageId { get; set; }

	public Guid? InvoiceId { get; set; }

	public AttachmentType Type { get; set; }
}