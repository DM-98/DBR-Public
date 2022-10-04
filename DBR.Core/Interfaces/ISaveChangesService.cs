namespace DBR.Core.Interfaces;

public interface ISaveChangesService
{
	Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}