namespace SortingSytem_V_26_05_23;

public class Worker : BackgroundService
{
    /*
     * Predefined by framework
     */
    
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Sorter.TickAll(Sorter.Sorters); 
            await Task.Delay(100, stoppingToken);
        }
    }
}