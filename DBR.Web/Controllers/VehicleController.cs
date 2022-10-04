using DBR.Core.Domain;
using DBR.Core.DTOs.Inputs;
using DBR.Core.DTOs.Outputs;
using DBR.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DBR.Web.Controllers;

[Route("api/vehicles")]
[ApiController]
public class VehicleController : EFBaseController<Vehicle, VehicleInputModel, VehicleDTO>
{
	public VehicleController(IService<Vehicle, VehicleInputModel, VehicleDTO> vehicleService) : base(vehicleService)
	{
	}
}