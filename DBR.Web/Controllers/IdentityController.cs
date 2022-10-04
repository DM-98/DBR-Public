using DBR.Core.Domain;
using DBR.Core.DTOs.Inputs;
using DBR.Core.DTOs.Outputs;
using DBR.Core.Enums;
using DBR.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DBR.Web.Controllers;

[Route("api/identity")]
[ApiController]
public class IdentityController : EFBaseController<Member, Member, MemberDTO>
{
	private readonly IAuthService authService;

	public IdentityController(IAuthService authService, IService<Member, Member, MemberDTO> memberService) : base(memberService)
	{
		this.authService = authService;
	}

	[AllowAnonymous]
	[HttpPost("register")]
	public async Task<ActionResult<ResponseDTO<Member>>> RegisterAsync(RegisterInputModel registerInputModel)
	{
		if (!ModelState.IsValid)
		{
			return ValidationProblem();
		}

		ResponseDTO<Member> responseDTO = await authService.RegisterAsync(registerInputModel);

		if (responseDTO.Success)
		{
			return CreatedAtAction(nameof(GetByIdAsync), new { id = (Guid)responseDTO.Content!.GetType().GetProperty("Id")?.GetValue(responseDTO.Content)! }, responseDTO.Content);
		}
		else
		{
			if (responseDTO.ErrorType is ErrorType.EntityDuplicate)
			{
				return Conflict(responseDTO);
			}
			else if (responseDTO.ErrorType is ErrorType.UserManagerError)
			{
				return UnprocessableEntity(responseDTO);
			}
			else
			{
				return StatusCode(500, responseDTO);
			}
		}
	}

	[AllowAnonymous]
	[HttpPost("login")]
	public async Task<ActionResult<ResponseDTO<Tuple<string, string>>>> LoginAsync(LoginInputModel loginInputModel)
	{
		if (!ModelState.IsValid)
		{
			return ValidationProblem();
		}

		ResponseDTO<Tuple<string, string>> responseDTO = await authService.LoginAsync(loginInputModel);

		if (responseDTO.Success)
		{
			return Ok(responseDTO);
		}
		else
		{
			if (responseDTO.ErrorType is ErrorType.EntityNotFound)
			{
				return NotFound(responseDTO);
			}
			else if (responseDTO.ErrorType is ErrorType.TooManyLoginAttempts)
			{
				return StatusCode(403, responseDTO);
			}
			else if (responseDTO.ErrorType is ErrorType.InvalidInput)
			{
				return BadRequest(responseDTO);
			}
			else if (responseDTO.ErrorType is ErrorType.UserManagerError)
			{
				return UnprocessableEntity(responseDTO);
			}
			else
			{
				return StatusCode(500, responseDTO);
			}
		}
	}

	[AllowAnonymous]
	[HttpPost("refresh-token")]
	public async Task<ActionResult<ResponseDTO<Tuple<string, string>>>> RefreshTokenAsync(Tuple<string, string> accessRefreshTokensToRefresh)
	{
		if (!ModelState.IsValid)
		{
			return ValidationProblem();
		}

		ResponseDTO<Tuple<string, string>> responseDTO = await authService.RefreshTokenAsync(accessRefreshTokensToRefresh);

		if (responseDTO.Success)
		{
			return Ok(responseDTO);
		}
		else
		{
			if (responseDTO.ErrorType is ErrorType.TokenNotFound)
			{
				return Unauthorized(responseDTO);
			}
			else if (responseDTO.ErrorType is ErrorType.ClaimsPrincipalNotFoundFromAccessToken)
			{
				return Unauthorized(responseDTO);
			}
			else if (responseDTO.ErrorType is ErrorType.UserNotFoundWithClaimsPrincipal)
			{
				return Unauthorized(responseDTO);
			}
			else if (responseDTO.ErrorType is ErrorType.RefreshTokenInvalidOrExpired)
			{
				return Unauthorized(responseDTO);
			}
			else if (responseDTO.ErrorType is ErrorType.UserManagerError)
			{
				return UnprocessableEntity(responseDTO);
			}
			else
			{
				return StatusCode(500, responseDTO);
			}
		}
	}

	[HttpPut("update-member")]
	public async Task<ActionResult<ResponseDTO<Tuple<string, string>>>> UpdateMemberAsync(Member updatedMember)
	{
		if (!ModelState.IsValid)
		{
			return ValidationProblem();
		}

		ResponseDTO<Tuple<string, string>> responseDTO = await authService.UpdateMemberAsync(updatedMember);

		if (responseDTO.Success)
		{
			return Ok(responseDTO);
		}
		else
		{
			if (responseDTO.ErrorType is ErrorType.UserManagerError)
			{
				return UnprocessableEntity(responseDTO);
			}
			else
			{
				return StatusCode(500, responseDTO);
			}
		}
	}

	[HttpPost("create-role/{role}")]
	public async Task<ActionResult<ResponseDTO<IdentityRole<Guid>>>> CreateRoleAsync(string role)
	{
		if (!ModelState.IsValid)
		{
			return ValidationProblem();
		}

		ResponseDTO<IdentityRole<Guid>> responseDTO = await authService.CreateRoleAsync(role);

		if (responseDTO.Success)
		{
			return Ok(responseDTO);
		}
		else
		{
			if (responseDTO.ErrorType is ErrorType.UserManagerError)
			{
				return UnprocessableEntity(responseDTO);
			}
			else
			{
				return StatusCode(500, responseDTO);
			}
		}
	}

	[HttpPut("update-role/{id}/{updatedRole}")]
	public async Task<ActionResult<ResponseDTO<IdentityRole<Guid>>>> UpdateRoleAsync(Guid id, string updatedRole)
	{
		if (!ModelState.IsValid)
		{
			return ValidationProblem();
		}

		ResponseDTO<IdentityRole<Guid>> responseDTO = await authService.UpdateRoleAsync(id, updatedRole);

		if (responseDTO.Success)
		{
			return Ok(responseDTO);
		}
		else
		{
			if (responseDTO.ErrorType is ErrorType.EntityNotFound)
			{
				return NotFound(responseDTO);
			}
			else if (responseDTO.ErrorType is ErrorType.UserManagerError)
			{
				return UnprocessableEntity(responseDTO);
			}
			else
			{
				return StatusCode(500, responseDTO);
			}
		}
	}

	[HttpDelete("delete-role/{role}")]
	public async Task<ActionResult<ResponseDTO<IdentityRole<Guid>>>> DeleteRoleAsync(string role)
	{
		if (!ModelState.IsValid)
		{
			return ValidationProblem();
		}

		ResponseDTO<IdentityRole<Guid>> responseDTO = await authService.DeleteRoleAsync(role);

		if (responseDTO.Success)
		{
			return Ok(responseDTO);
		}
		else
		{
			if (responseDTO.ErrorType is ErrorType.EntityNotFound)
			{
				return NotFound(responseDTO);
			}
			else if (responseDTO.ErrorType is ErrorType.UserManagerError)
			{
				return UnprocessableEntity(responseDTO);
			}
			else
			{
				return StatusCode(500, responseDTO);
			}
		}
	}

	[HttpPost("revoke-member/{id}")]
	public async Task<ActionResult<ResponseDTO<Member>>> RevokeMemberByIdAsync(Guid id)
	{
		if (!ModelState.IsValid)
		{
			return ValidationProblem();
		}

		ResponseDTO<Member> responseDTO = await authService.RevokeMemberByIdAsync(id);

		if (responseDTO.Success)
		{
			return Ok(responseDTO);
		}
		else
		{
			if (responseDTO.ErrorType is ErrorType.EntityNotFound)
			{
				return NotFound(responseDTO);
			}
			else if (responseDTO.ErrorType is ErrorType.UnableToRevokeMember)
			{
				return UnprocessableEntity(responseDTO);
			}
			else
			{
				return StatusCode(500, responseDTO);
			}
		}
	}

	[HttpPost("revoke-all")]
	public async Task<ActionResult<ResponseDTO<Member>>> RevokeAllMembersAsync(CancellationToken cancellationToken)
	{
		ResponseDTO<Member> responseDTO = await authService.RevokeAllMembersAsync(cancellationToken);

		if (responseDTO.Success)
		{
			return Ok(responseDTO);
		}
		else
		{
			if (responseDTO.ErrorType is ErrorType.CancellationTokenRequested)
			{
				return StatusCode(499, responseDTO);
			}
			else if (responseDTO.ErrorType is ErrorType.UnableToRevokeMember)
			{
				return UnprocessableEntity(responseDTO);
			}
			else
			{
				return StatusCode(500, responseDTO);
			}
		}
	}
}