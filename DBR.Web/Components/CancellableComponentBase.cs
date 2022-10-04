using Microsoft.AspNetCore.Components;

namespace DBR.Web.Components;

public class CancellableComponentBase : ComponentBase, IDisposable
{
	private readonly CancellationTokenSource cancellationSource = new();

	protected CancellationToken CancellationToken => cancellationSource.Token;

	public void Dispose()
	{
		cancellationSource.Cancel();
		cancellationSource.Dispose();
		GC.SuppressFinalize(this);
	}
}