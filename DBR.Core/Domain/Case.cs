using System.ComponentModel.DataAnnotations.Schema;
using DBR.Core.Enums;

namespace DBR.Core.Domain;

[Table("Cases")]
public class Case : BaseEntity
{
	public Guid WorkshopId { get; set; }

	[ForeignKey(nameof(WorkshopId))]
	public Workshop? Workshop { get; set; }

	public Guid CustomerId { get; set; }

	[ForeignKey(nameof(CustomerId))]
	public Customer? Customer { get; set; }

	public CaseStatus Status { get; set; }

	public ICollection<Incident>? Incidents { get; set; }
}