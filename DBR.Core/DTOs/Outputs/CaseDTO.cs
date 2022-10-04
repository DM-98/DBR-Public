using DBR.Core.Domain;
using DBR.Core.Enums;

namespace DBR.Core.DTOs.Outputs;

public class CaseDTO : BaseEntity
{
	public WorkshopDTO? Workshop { get; set; }

	public CustomerDTO? Customer { get; set; }

	public CaseStatus Status { get; set; }

	public IEnumerable<IncidentDTO>? Incidents { get; set; }
}