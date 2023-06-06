using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Sorting_Service_13_02_2023_1_0
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            string EnvPth = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            
            if (File.Exists(EnvPth + @"\Sorters\Sorters.json"))
            {
                Sorter.Sorters = Sorter.loadSorters(EnvPth + @"\Sorters\Sorters.json");
                //Sorter.startSorters(Sorter.Sorters);
            }
            else
            {
                Console.WriteLine(@"Error: At least one sorter is required, install one at 'Users\Documents\Sorters\Sorters.json'");
                Environment.Exit(-1);
            }
            
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                foreach (Sorter s in Sorter.Sorters)
                {
                    s.OnTimed();
                }
                await Task.Delay(1, stoppingToken);
            }
        }
    }
}