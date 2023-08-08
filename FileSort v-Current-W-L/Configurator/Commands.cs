using System.Reflection;
using SortingSystem;

namespace Configurator;

public static class Commands
{
    public static void CommandDetermination(List<string> args)
    {
        string command = args[0];
        args.RemoveAt(0);
        string value = "";
        args.ForEach(x => value += x);
        
        try
        {
            MethodInfo theMethod = typeof(Commands).GetMethod(command,
                BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase);
            object[] parameters = new object[] { };

            if (args.Count > 0)
                parameters = new object[] { value.Split("-") };

            theMethod.Invoke(null, parameters);
        }
        catch (NullReferenceException nullReferenceException)
        {
            Console.WriteLine("Unknown command or command arguments '" + command + value + "'" + ", type '-h' for help" );   
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    public static void Search(string[] commandArgs) 
    {
        string searchValue = commandArgs[0];
        
        for (int i = 1; i < commandArgs.Length; i++)
        {
            
            
            
            if (commandArgs[i].StartsWith("filter") || commandArgs[i].StartsWith("f"))
            {
                commandArgs[i] = commandArgs[i].Replace("filter", "").Replace("f", "").Trim();
                string[] values = commandArgs[i].Split(" ");
                Enum.TryParse(values[0],true, out FilterBy filterBy);
                
                if (Sorter.CategoryExists(values[1],Sorter.Sorters))
                {
                    int neededCompValue = -99;
                    string filterValue = values[1];
                    SortingHistory.SortedItems = SortingHistory.FilterListBy(SortingHistory.SortedItems, filterBy, neededCompValue, filterValue); 
                }
                else
                {
                    int neededCompValue = Utils.compSymbolToInt(values[1][0]);
                    string filterValue = values[1].Substring(1);
                    SortingHistory.SortedItems = SortingHistory.FilterListBy(SortingHistory.SortedItems, filterBy, neededCompValue, filterValue); 
                }
            }
            else if (commandArgs[i].StartsWith("sort") || commandArgs[i].StartsWith("s"))
            {
                commandArgs[i] = commandArgs[i].Replace("sort", "").Replace("s", "").Trim();
                string[] values = commandArgs[i].Split(" ");

                bool reverse = values[0][0] == '!';
                Enum.TryParse(values[0].Replace('!',' '),true, out SortBy sortBy);
                SortingHistory.SortedItems = SortingHistory.SortListBy(SortingHistory.SortedItems, sortBy, reverse);
            }
            else
            {
                throw new Exception("Wrong input format, type -h for help");
            }
            
        }
        
        List<SortedItem> itemsToSearch = SortingHistory.SearchListBy(SortingHistory.SortedItems,searchValue);
        SortingHistory.ToTable(itemsToSearch);
    }

    public static void Create()
    {
        Console.Write(File.ReadAllText(@"Messages\createFont.txt"));
        
        try
        {
            string name = Configurator.AddName();
            string scanDir = Configurator.AddScanDir();
            string endDir = Configurator.AddEndDir();
            List<Extension> exts = Configurator.AddExtensionsMenu(new List<Extension>());
            string[] staticDirs = Configurator.AddStaticDirs(scanDir);
            bool deleteFromDesktop = ConsoleUtils.YesNoQuery("\nDo you want to delete your items from desktop? (Y/N)");
            bool sortByYearMonth = ConsoleUtils.YesNoQuery("\nDo you want to sort by year and month? (Y/N)" );
                
            Sorter.Sorters.Add(new Sorter(name,scanDir,endDir,exts,staticDirs,deleteFromDesktop,sortByYearMonth));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public static void Edit()
    {
        Console.Write(File.ReadAllText(@"Messages\editFont.txt"));
        
        if (Sorter.Sorters.Count == 0 )
            return;
            
        List<string> names = new List<string>();
        Sorter.Sorters.ForEach(x => names.Add(x.Name));
        int sel = ConsoleUtils.OptionSelectQuery("\nSelect a Sorter",names.ToArray());
        bool exit = false;
            
        do
        {
            int sel2 = ConsoleUtils.OptionSelectQuery("\nSelect an option",
                File.ReadAllLines("Messages/editOptions.txt"))+1;

            switch (sel2)
            {
                case 1:
                    Sorter.Sorters[sel].Name = Configurator.AddName();
                    break;
                case 2:
                    Sorter.Sorters[sel].ScanDir = Configurator.AddScanDir();
                    break;
                case 3:
                    Sorter.Sorters[sel].EndDir = Configurator.AddEndDir();
                    break;
                case 4:
                    Sorter.Sorters[sel].Extensions = Configurator.AddExtensionsMenu(Sorter.Sorters[sel].Extensions);
                    break;
                case 5:
                    Sorter.Sorters[sel].StaticDirs = Configurator.AddStaticDirs(Sorter.Sorters[sel].ScanDir);
                    break;
                case 6:
                    Sorter.Sorters[sel].DeleteFromDesktop = ConsoleUtils.YesNoQuery("\nDo you want to delete items from desktop? (y/n)");
                    break;
                case 7:
                    exit = true;
                    break;
            }
        } while (!exit);
    }
    
    public static void Update()
    {
        
    }

    public static void Delete()
    {
        Console.Write(File.ReadAllText(@"Messages\deleteFont.txt"));
        
        if (Sorter.Sorters.Count == 0)
            return;
            
        List<string> names = new List<string>();
        Sorter.Sorters.ForEach(x => names.Add(x.Name));
        int sel = ConsoleUtils.OptionSelectQuery("\nSelect the sorter you want to delete",names.ToArray());
            
        Sorter.Sorters.RemoveAt(sel);
    }

    public static void Help()
    {
        Type thisType = typeof(Commands);
        MethodInfo[] theMethods = thisType.GetMethods();
        
        foreach (MethodInfo methodInfo in theMethods)
        {
            Console.WriteLine(methodInfo.Name );
            methodInfo.GetParameters().ToList().ForEach(x => Console.Write(x.Name));
        }
    }




}