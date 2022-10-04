using DBR.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DBR.Infrastructure.Services;

public class SaveChangesService<TDbContext> : ISaveChangesService where TDbContext : DbContext
{
	private readonly TDbContext context;

	public SaveChangesService(TDbContext context)
	{
		this.context = context;
	}

	public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
	{
		return await context.SaveChangesAsync(cancellationToken);
	}
}