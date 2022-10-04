using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DBR.Core.Enums;

namespace DBR.Core.Domain;

[Table("Incidents")]
public class Incident : BaseEntity
{
	public Guid CaseId { get; set; }

	[ForeignKey(nameof(CaseId))]
	public Case? Case { get; set; }

	[StringLength(20)]
	public string? ShortId { get; set; }

	[StringLength(500)]
	public string Description { get; set; } = null!;

	public IncidentType Type { get; set; }

	public ICollection<Attachment>? Attachments { get; set; }
}