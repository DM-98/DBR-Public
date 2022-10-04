using DBR.Core.Domain;
using DBR.Core.Enums;

namespace DBR.Core.DTOs.Outputs;

public class AttachmentDTO : BaseEntity
{
	public IncidentDTO? Incident { get; set; }

	public VideoDTO? Video { get; set; }

	public ImageDTO? Image { get; set; }

	public InvoiceDTO? Invoice { get; set; }

	public AttachmentType Type { get; set; }
}