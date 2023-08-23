using System.Diagnostics;
using System.Net.Mime;
using System.Reflection;
using System.Text.RegularExpressions;
using SortingSystem;

namespace Configurator
{
    public static class Program
    {
        public static bool Exit { get; set; }


        static void Main(string[] args)
        {
            try
            {
                AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
                Logger.Init("Logs", System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
                SortingHistory.SortedItems = SortingHistory.LoadHistory();
                
                if (!Utils.IsAdministrator())
                    throw new Exception("Please run the configurator as administrator");
                
                Sorter.Sorters = new List<Sorter>();
                
                if (File.Exists("Sorters.json"))
                    Sorter.Sorters = Sorter.loadSorters("Sorters.json");
                
                if(args.Length > 0)
                    Commands.CommandDetermination(args.ToList());
                else
                    OpenConfigurator();
            }
            catch (Exception e)
            {
                Logger.Add(e.Message + e.StackTrace);
            }

        }
        
        public static void OpenConfigurator()
        {
            Console.Write(File.ReadAllText("Messages/WelcomeMessage.txt"));
            FileSort.Stop();
            Exit = false;
            
            while(!Exit)
            {
                Console.Write("\n[Terminal] ");
                string[] inputArgs = Console.ReadLine().Split(" ");
                Commands.CommandDetermination(inputArgs.ToList());
            }
            
            Sorter.saveSorters(Sorter.Sorters,"Sorters.json");
            FileSort.Start();
        }

        public static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            if(Sorter.Sorters != null)
                Sorter.saveSorters(Sorter.Sorters,"Sorters.json");
            FileSort.Start();
        }
        
 
    }
}