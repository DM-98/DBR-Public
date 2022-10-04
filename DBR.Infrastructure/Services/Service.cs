using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using AutoMapper;
using DBR.Core.DTOs.Outputs;
using DBR.Core.Enums;
using DBR.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;

namespace DBR.Infrastructure.Services;

public class Service<T, TIN, TOUT> : IService<T, TIN, TOUT> where T : class where TIN : class where TOUT : class
{
	private readonly IRepository<T> repository;
	private readonly ISaveChangesService saveChangesService;
	private readonly IMapper mapper;

	public Service(IRepository<T> repository, ISaveChangesService saveChangesService, IMapper mapper)
	{
		this.repository = repository;
		this.saveChangesService = saveChangesService;
		this.mapper = mapper;
	}

	public async IAsyncEnumerable<ResponseDTO<TOUT>> GetAsync([EnumeratorCancellation] CancellationToken cancellationToken, bool isTrackingDisabled = true, Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includeProperties = null, Expression<Func<T, T>>? selector = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
	{
		await foreach (T entity in repository.Get(isTrackingDisabled, filter, includeProperties, selector, orderBy).AsAsyncEnumerable())
		{
			if (cancellationToken.IsCancellationRequested)
			{
				yield return new ResponseDTO<TOUT> { Success = false, ErrorMessage = "Din anmodning blev annulleret.", ErrorType = ErrorType.CancellationTokenRequested };
				yield break;
			}

			TOUT entityDTO = mapper.Map<TOUT>(entity);

			yield return new ResponseDTO<TOUT> { Success = true, Content = entityDTO };
		}
	}

	public async Task<ResponseDTO<TOUT>> GetByIdAsync(Guid id, CancellationToken cancellationToken, bool isTrackingDisabled = true, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includeProperties = null, Expression<Func<T, T>>? selector = null)
	{
		try
		{
			List<T> entities = await repository.Get(isTrackingDisabled: isTrackingDisabled, includeProperties: includeProperties, selector: selector).ToListAsync(cancellationToken);
			T? entity = entities.Find(entity => (Guid)entity.GetType().GetProperty("Id")!.GetValue(entity)! == id);

			if (entity is null)
			{
				return new ResponseDTO<TOUT> { Success = false, ErrorMessage = "Din anmodning blev ikke fundet - prøv igen.", ErrorType = ErrorType.EntityNotFound };
			}

			TOUT entityDTO = mapper.Map<TOUT>(entity);

			return new ResponseDTO<TOUT> { Success = true, Content = entityDTO };
		}
		catch (NullReferenceException ex)
		{
			return new ResponseDTO<TOUT> { Success = false, ErrorMessage = "Null reference exception skete.", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = ErrorType.CancellationTokenRequested };
		}
		catch (OperationCanceledException ex)
		{
			return new ResponseDTO<TOUT> { Success = false, ErrorMessage = "Din anmodning blev annulleret.", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = ErrorType.CancellationTokenRequested };
		}
		catch (Exception ex)
		{
			return new ResponseDTO<TOUT> { Success = false, ErrorMessage = "Ubehandlet serverfejl. Kontakt server administrator.", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = ErrorType.Unhandled };
		}
	}

	public async Task<ResponseDTO<TOUT>> CreateAsync(TIN entityInputModel, CancellationToken cancellationToken)
	{
		try
		{
			T entityToCreate = mapper.Map<T>(entityInputModel);
			T createdEntity = repository.Create(entityToCreate);

			await saveChangesService.SaveChangesAsync(cancellationToken);

			TOUT createdEntityDTO = mapper.Map<TOUT>(createdEntity);

			return new ResponseDTO<TOUT> { Success = true, Content = createdEntityDTO };
		}
		catch (OperationCanceledException ex)
		{
			return new ResponseDTO<TOUT> { Success = false, ErrorMessage = "Din anmodning blev annulleret.", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = ErrorType.CancellationTokenRequested };
		}
		catch (Exception ex)
		{
			return new ResponseDTO<TOUT> { Success = false, ErrorMessage = "Ubehandlet serverfejl. Kontakt server administrator.", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = ErrorType.Unhandled };
		}
	}

	public async Task<ResponseDTO<TOUT>> UpdateAsync(TOUT entityDTO, CancellationToken cancellationToken)
	{
		try
		{
			T entityToUpdate = (await repository.Get().ToListAsync(cancellationToken)).Find(x => (Guid)x.GetType().GetProperty("Id")!.GetValue(x)! == (Guid)entityDTO.GetType().GetProperty("Id")!.GetValue(entityDTO)!)!;
			T mappedEntity = mapper.Map(entityDTO, entityToUpdate);
			T updatedEntity = repository.Update(mappedEntity);

			await saveChangesService.SaveChangesAsync(cancellationToken);

			TOUT updatedEntityDTO = mapper.Map<TOUT>(updatedEntity);

			return new ResponseDTO<TOUT> { Success = true, Content = updatedEntityDTO };
		}
		catch (OperationCanceledException ex)
		{
			return new ResponseDTO<TOUT> { Success = false, ErrorMessage = "Din anmodning blev annulleret.", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = ErrorType.CancellationTokenRequested };
		}
		catch (DbUpdateConcurrencyException ex)
		{
			EntityEntry entityEntry = ex.Entries.Single();
			PropertyValues? entityDbValues = entityEntry.GetDatabaseValues();

			if (entityDbValues is null)
			{
				return new ResponseDTO<TOUT> { Success = false, ErrorMessage = $"Konflikt ved redigering - objektet findes ikke længere i databasen, da en anden har slettet den imellemtiden.", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = ErrorType.OptimisticConcurrency };
			}

			T dbEntity = (T)entityDbValues.ToObject();
			TOUT dbEntityDTO = mapper.Map<TOUT>(dbEntity);

			return new ResponseDTO<TOUT> { Success = false, ErrorMessage = "Konflikt ved redigering - en anden har redigeret på denne før dig. Tjek felterne, bekræft værdierne/ret til og anmod om redigering igen.", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = ErrorType.OptimisticConcurrency, Content = dbEntityDTO };
		}
		catch (Exception ex)
		{
			return new ResponseDTO<TOUT> { Success = false, ErrorMessage = "Ubehandlet serverfejl. Kontakt server administrator.", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = ErrorType.Unhandled };
		}
	}

	public async Task<ResponseDTO<TOUT>> DeleteAsync(CancellationToken cancellationToken, Guid? id = default, TOUT? entityDTO = default)
	{
		try
		{
			T? entityToDelete;

			if (entityDTO is not null)
			{
				entityToDelete = mapper.Map<T>(entityDTO);
			}
			else
			{
				IQueryable<T> query = repository.Get(filter: entity => (Guid)entity.GetType().GetProperty("Id")!.GetValue(entity)! == id);

				entityToDelete = await query.FirstOrDefaultAsync();

				if (entityToDelete is null)
				{
					return new ResponseDTO<TOUT> { Success = false, ErrorMessage = "Din anmodning blev ikke fundet - prøv igen.", ErrorType = ErrorType.EntityNotFound };
				}
			}

			T deletedEntity = repository.Delete(entityToDelete);

			await saveChangesService.SaveChangesAsync(cancellationToken);

			TOUT deletedEntityDTO = mapper.Map<TOUT>(deletedEntity);

			return new ResponseDTO<TOUT> { Success = true, Content = deletedEntityDTO };
		}
		catch (OperationCanceledException ex)
		{
			return new ResponseDTO<TOUT> { Success = false, ErrorMessage = "Din anmodning blev annulleret.", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = ErrorType.CancellationTokenRequested };
		}
		catch (Exception ex)
		{
			return new ResponseDTO<TOUT> { Success = false, ErrorMessage = "Ubehandlet serverfejl. Kontakt server administrator.", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = ErrorType.Unhandled };
		}
	}
}