using System.Linq.Expressions;
using DBR.Core.DTOs.Outputs;
using Microsoft.EntityFrameworkCore.Query;

namespace DBR.Core.Interfaces;

public interface IService<T, TIN, TOUT> where T : class where TIN : class where TOUT : class
{
	IAsyncEnumerable<ResponseDTO<TOUT>> GetAsync(CancellationToken cancellationToken, bool isTrackingDisabled = true, Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includeProperties = null, Expression<Func<T, T>>? selector = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);

	Task<ResponseDTO<TOUT>> GetByIdAsync(Guid id, CancellationToken cancellationToken, bool isTrackingDisabled = true, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includeProperties = null, Expression<Func<T, T>>? selector = null);

	Task<ResponseDTO<TOUT>> CreateAsync(TIN entityInputModel, CancellationToken cancellationToken);

	Task<ResponseDTO<TOUT>> UpdateAsync(TOUT entityDTO, CancellationToken cancellationToken);

	Task<ResponseDTO<TOUT>> DeleteAsync(CancellationToken cancellationToken, Guid? id = default, TOUT? entityDTO = default);
}