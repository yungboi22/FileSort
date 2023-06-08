using System.Diagnostics.Metrics;
using System.Net;
using System.Runtime.InteropServices;
using SortingSytem_V_26_05_23;


Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);
File.WriteAllText(@"D:\Log.txt",Directory.GetCurrentDirectory());

IOMethod.Init();
Sorter.Sorters = new List<Sorter>();
string Loc = File.ReadAllText("SaveLoc").Trim();
Sorter.Sorters = Sorter.loadSorters(Loc);
Sorter.startSorters(Sorter.Sorters);

IHost host;

if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
{
    host = Host.CreateDefaultBuilder(args)
        .UseSystemd()
        .ConfigureServices(services => { services.AddHostedService<Worker>(); })
        .Build();
}
else
{
    host = Host.CreateDefaultBuilder(args)
        .UseWindowsService()
        .ConfigureServices(services => { services.AddHostedService<Worker>(); })
        .Build();
}


await host.RunAsync();