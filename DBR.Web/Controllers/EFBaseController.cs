using System.Runtime.CompilerServices;
using DBR.Core.DTOs.Outputs;
using DBR.Core.Enums;
using DBR.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DBR.Web.Controllers;

[Authorize(Roles = "Admin")]
public abstract class EFBaseController<T, TIN, TOUT> : ControllerBase where T : class where TIN : class where TOUT : class
{
	private readonly IService<T, TIN, TOUT> service;

	public EFBaseController(IService<T, TIN, TOUT> service)
	{
		this.service = service;
	}

	[HttpGet]
	public async IAsyncEnumerable<ResponseDTO<TOUT>> GetAllAsync([EnumeratorCancellation] CancellationToken cancellationToken)
	{
		IAsyncEnumerable<ResponseDTO<TOUT>> response = service.GetAsync(cancellationToken);

		await foreach (ResponseDTO<TOUT> responseDTO in response)
		{
			yield return responseDTO;
		}
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<ResponseDTO<TOUT>>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
	{
		ResponseDTO<TOUT> responseDTO = await service.GetByIdAsync(id, cancellationToken);

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
			else if (responseDTO.ErrorType is ErrorType.EntityNotFound)
			{
				return NotFound(responseDTO);
			}
			else
			{
				return StatusCode(500, responseDTO);
			}
		}
	}

	[HttpPost]
	public async Task<ActionResult<ResponseDTO<TOUT>>> CreateAsync(TIN entityInputModel, CancellationToken cancellationToken)
	{
		if (!ModelState.IsValid)
		{
			return ValidationProblem();
		}

		ResponseDTO<TOUT> responseDTO = await service.CreateAsync(entityInputModel, cancellationToken);

		if (responseDTO.Success)
		{
			return CreatedAtAction(nameof(GetByIdAsync), new { id = (Guid)responseDTO.Content!.GetType().GetProperty("Id")!.GetValue(responseDTO.Content)! }, responseDTO.Content);
		}
		else
		{
			if (responseDTO.ErrorType is ErrorType.CancellationTokenRequested)
			{
				return StatusCode(499, responseDTO);
			}
			else
			{
				return StatusCode(500, responseDTO);
			}
		}
	}

	[HttpPut]
	public async Task<ActionResult<ResponseDTO<TOUT>>> UpdateAsync(TOUT updatedEntity, CancellationToken cancellationToken)
	{
		if (!ModelState.IsValid)
		{
			return ValidationProblem();
		}

		ResponseDTO<TOUT> responseDTO = await service.UpdateAsync(updatedEntity, cancellationToken: cancellationToken);

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
			else if (responseDTO.ErrorType is ErrorType.OptimisticConcurrency)
			{
				return Conflict(responseDTO);
			}
			else
			{
				return StatusCode(500, responseDTO);
			}
		}
	}

	[HttpDelete("{id?}")]
	public async Task<ActionResult<ResponseDTO<TOUT>>> DeleteAsync(CancellationToken cancellationToken, Guid? id = default, TOUT? entityDTO = default)
	{
		ResponseDTO<TOUT> responseDTO = entityDTO is not null
			? await service.DeleteAsync(entityDTO: entityDTO, cancellationToken: cancellationToken)
			: await service.DeleteAsync(id: id, cancellationToken: cancellationToken);

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
			else if (responseDTO.ErrorType is ErrorType.OptimisticConcurrency)
			{
				return Conflict(responseDTO);
			}
			else
			{
				return StatusCode(500, responseDTO);
			}
		}
	}
}