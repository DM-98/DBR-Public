using DBR.Core.Domain;
using DBR.Core.DTOs.Inputs;
using DBR.Core.DTOs.Outputs;
using DBR.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DBR.Web.Controllers;

[Route("api/addresses")]
[ApiController]
public class AddressController : EFBaseController<Address, AddressInputModel, AddressDTO>
{
	public AddressController(IService<Address, AddressInputModel, AddressDTO> addressService) : base(addressService)
	{
	}
}