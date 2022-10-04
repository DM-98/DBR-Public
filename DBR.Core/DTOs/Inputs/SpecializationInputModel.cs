using System.ComponentModel.DataAnnotations;

namespace DBR.Core.DTOs.Inputs;

public class SpecializationInputModel
{
	[Required(ErrorMessage = "Dette felt skal udfyldes.")]
	[StringLength(25, ErrorMessage = "{0} kan højest indeholde {1} tegn.")]
	[Display(Name = "Specialiseringer")]
	public string Name { get; set; } = null!;
}