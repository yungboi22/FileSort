using System.Net.Mime;
using Microsoft.Extensions.Hosting.WindowsServices;

using Sortingsystem_Service;


public class Program
{
    public static List<Sorter> Sorters = new List<Sorter>();
    
    public static void Main(string[] args)
    {
        if (File.Exists(@"Sorters\Sorters.json"))
        {
            Sorters = Sorter.loadSorters(@"Sorters\Sorters.json");
        }
        else
        {
            Environment.Exit(-5);
        }
        
        IHost host = Host.CreateDefaultBuilder(args)
            .UseWindowsService()
            .ConfigureServices(services => { services.AddHostedService<Worker>(); })
            .Build();

        host.Run(); 
        
    }
}


