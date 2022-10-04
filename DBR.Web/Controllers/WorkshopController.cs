using DBR.Core.Domain;
using DBR.Core.DTOs.Inputs;
using DBR.Core.DTOs.Outputs;
using DBR.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DBR.Web.Controllers;

[Route("api/workshops")]
[ApiController]
public class WorkshopController : EFBaseController<Workshop, WorkshopInputModel, WorkshopDTO>
{
	public WorkshopController(IService<Workshop, WorkshopInputModel, WorkshopDTO> workshopService) : base(workshopService)
	{
	}
}