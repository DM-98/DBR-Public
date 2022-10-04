using DBR.Core.Domain;
using DBR.Core.DTOs.Inputs;
using DBR.Core.DTOs.Outputs;
using DBR.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DBR.Web.Controllers;

[Route("api/cases")]
[ApiController]
public class CaseController : EFBaseController<Case, CaseInputModel, CaseDTO>
{
	public CaseController(IService<Case, CaseInputModel, CaseDTO> caseService) : base(caseService)
	{
	}
}