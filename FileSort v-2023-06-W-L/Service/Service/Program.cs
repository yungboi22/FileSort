using System.Diagnostics.Metrics;
using System.Net;
using System.Runtime.InteropServices;

using SortingSytem_V_26_05_23;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            Logger.Init("Logs");
            IOMethod.Init();
            
            
            Sorter.Sorters = Sorter.loadSorters("Sorters.json");
            Sorter.enableSorters(Sorter.Sorters);
            
            IHost host = CreateHostBuilder(args);
            host.Run();
        }
        catch (Exception e)
        {
            Logger.Add(e.Message + "\n" + e.StackTrace);
        }
    }

    public static IHost CreateHostBuilder(string[] args)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return Host.CreateDefaultBuilder(args)
                .UseSystemd()
                .ConfigureServices(services => { services.AddHostedService<Worker>(); })
                .Build();
        else
            return Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices(services => { services.AddHostedService<Worker>(); })
                .Build();
                
    }
}













