using System.ComponentModel.DataAnnotations;
using DBR.Core.Enums;

namespace DBR.Core.DTOs.Inputs;

public class CaseInputModel
{
	public Guid CustomerId { get; set; }

	public Guid WorkshopId { get; set; }

	[Display(Name = "Status")]
	public string Status { get; set; } = CaseStatus.Active.ToString();

	[ValidateComplexType]
	public VehicleInputModel VehicleInputModel { get; set; }

	[ValidateComplexType]
	public CustomerInputModel CustomerInputModel { get; set; }

	public CaseInputModel()
	{
		VehicleInputModel = new();
		CustomerInputModel = new();
	}
}