using System.ComponentModel.DataAnnotations;
using DBR.Core.Domain;

namespace DBR.Core.DTOs.Outputs;

public class WorkshopDTO : BaseEntity
{
	[Required(ErrorMessage = "Dette felt skal udfyldes.")]
	[StringLength(50, ErrorMessage = "{0} kan højest indeholde {1} tegn.")]
	[Display(Name = "Navn")]
	public string Name { get; set; } = null!;

	[ValidateComplexType]
	public AddressDTO? Address { get; set; }

	[Required(ErrorMessage = "Dette felt skal udfyldes.")]
	[StringLength(15, MinimumLength = 8, ErrorMessage = "{0} skal mindst være {1} tegn, og kan højest være {2} tegn.")]
	[Display(Name = "Telefonnummer")]
	public string PhoneNumber { get; set; } = null!;

	public IEnumerable<Member>? Members { get; set; }

	public IEnumerable<CaseDTO>? Cases { get; set; }

	public IEnumerable<SpecializationDTO>? Specializations { get; set; }
}