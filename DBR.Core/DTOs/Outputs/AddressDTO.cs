using System.ComponentModel.DataAnnotations;
using DBR.Core.Domain;

namespace DBR.Core.DTOs.Outputs;

public class AddressDTO : BaseEntity
{
	[Required(ErrorMessage = "Dette felt skal udfyldes.")]
	[StringLength(50, ErrorMessage = "{0} kan højest indeholde {1} tegn.")]
	[Display(Name = "Gadenavn og husnummer")]
	public string Street { get; set; } = null!;

	[Required(ErrorMessage = "Dette felt skal udfyldes.")]
	[StringLength(25, ErrorMessage = "{0} kan højest indeholde {1} tegn.")]
	[Display(Name = "By")]
	public string City { get; set; } = null!;

	[Required(ErrorMessage = "Dette felt skal udfyldes.")]
	[Display(Name = "Postnummer")]
	public short? PostCode { get; set; }
}