using DBR.Core.Domain;
using DBR.Core.DTOs.Inputs;
using DBR.Core.DTOs.Outputs;
using DBR.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DBR.Web.Controllers;

[Route("api/incidents")]
[ApiController]
public class IncidentController : EFBaseController<Incident, IncidentInputModel, IncidentDTO>
{
	public IncidentController(IService<Incident, IncidentInputModel, IncidentDTO> incidentService) : base(incidentService)
	{
	}
}