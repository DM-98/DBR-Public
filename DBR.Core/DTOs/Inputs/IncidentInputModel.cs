using System.ComponentModel.DataAnnotations;
using DBR.Core.Enums;

namespace DBR.Core.DTOs.Inputs;

public class IncidentInputModel
{
	[Display(Name = "Hændelsestype")]
	public IncidentType Type { get; set; } = IncidentType.InfoIntern;

	[Required(ErrorMessage = "Dette felt skal udfyldes.")]
	[StringLength(500, ErrorMessage = "{0} kan højest indeholde {1} tegn.")]
	[Display(Name = "Beskrivelse*")]
	public string Description { get; set; } = null!;

	public Guid CaseId { get; set; }
}