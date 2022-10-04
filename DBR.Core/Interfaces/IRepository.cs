using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace DBR.Core.Interfaces;

public interface IRepository<T> where T : class
{
	IQueryable<T> Get(bool isTrackingDisabled = true, Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includeProperties = null, Expression<Func<T, T>>? selector = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);

	T Create(T entity);

	T Update(T entity);

	T Delete(T entity);
}