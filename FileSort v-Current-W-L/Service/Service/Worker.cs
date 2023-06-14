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

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        Logger.Add("Starting service at " + "\"" + Directory.GetCurrentDirectory() + "\"...");
        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        Logger.Add("Shutting down service... \n");
        return base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Sorter.TickAll(Sorter.Sorters); 
                await Task.Delay(100, stoppingToken);
            }
        }
        catch (Exception e)
        {
            Logger.Add(e.Message + "\n" + e.StackTrace);
            throw;
        }
        
    }
}