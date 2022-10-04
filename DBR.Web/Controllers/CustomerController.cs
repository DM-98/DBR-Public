using DBR.Core.Domain;
using DBR.Core.DTOs.Inputs;
using DBR.Core.DTOs.Outputs;
using DBR.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DBR.Web.Controllers;

[Route("api/customers")]
[ApiController]
public class CustomerController : EFBaseController<Customer, CustomerInputModel, CustomerDTO>
{
	public CustomerController(IService<Customer, CustomerInputModel, CustomerDTO> customerService) : base(customerService)
	{
	}
}