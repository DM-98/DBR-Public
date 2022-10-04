using System.ComponentModel.DataAnnotations.Schema;
using DBR.Core.Enums;

namespace DBR.Core.Domain;

[Table("Attachments")]
public class Attachment : BaseEntity
{
	public Guid IncidentId { get; set; }

	[ForeignKey(nameof(IncidentId))]
	public Incident? Incident { get; set; }

	public Guid? VideoId { get; set; }

	[ForeignKey(nameof(VideoId))]
	public Video? Video { get; set; }

	public Guid? ImageId { get; set; }

	[ForeignKey(nameof(ImageId))]
	public Image? Image { get; set; }

	public Guid? InvoiceId { get; set; }

	[ForeignKey(nameof(InvoiceId))]
	public Invoice? Invoice { get; set; }

	public AttachmentType Type { get; set; }
}