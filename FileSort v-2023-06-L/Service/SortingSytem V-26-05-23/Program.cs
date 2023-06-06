using System.Diagnostics.Metrics;
using System.Net;
using SortingSytem_V_26_05_23;

IOMethod.Init();
Sorter.Sorters = new List<Sorter>();
string Loc = File.ReadAllText("SaveLoc").Trim();
Sorter.Sorters = Sorter.loadSorters(Loc);
Sorter.startSorters(Sorter.Sorters);

IHost host = Host.CreateDefaultBuilder(args)
    .UseSystemd()
    .ConfigureServices(services => { services.AddHostedService<Worker>(); })
    .Build();

await host.RunAsync();