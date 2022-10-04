using System.Linq.Expressions;
using DBR.Core.Interfaces;
using DBR.Core.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace DBR.Infrastructure.Repositories;

public class EFRepository<T, TDbContext> : IRepository<T> where T : class where TDbContext : DbContext
{
	private readonly TDbContext context;
	private readonly DbSet<T> table;

	public EFRepository(TDbContext context)
	{
		this.context = context;
		table = this.context.Set<T>();
	}

	public IQueryable<T> Get(bool isTrackingDisabled = true, Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includeProperties = null, Expression<Func<T, T>>? selector = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
	{
		return table.AsQueryable().QueryHelper(isTrackingDisabled, filter, includeProperties, selector, orderBy);
	}

	public T Create(T entity)
	{
		return table.Add(entity).Entity;
	}

	public T Update(T entity)
	{
		context.ChangeTracker.Clear();

		context.Entry(entity).Property("UpdatedDate").CurrentValue = DateTime.UtcNow;
		context.Entry(entity).State = EntityState.Modified;
		context.Entry(entity).Property("CreatedDate").IsModified = false;

		return entity;
	}

	public T Delete(T entity)
	{
		return table.Remove(entity).Entity;
	}
}