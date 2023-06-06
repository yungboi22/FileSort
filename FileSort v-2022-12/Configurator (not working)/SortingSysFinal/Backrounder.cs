using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SortingSysFinal;

public class Backrounder:BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stopToken)
    {
        while (!stopToken.IsCancellationRequested)
        {
            await Task.Delay(100,stopToken);
        }
    }
    
}