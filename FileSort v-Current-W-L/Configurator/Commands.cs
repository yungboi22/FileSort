using System.Diagnostics;
using System.Net;
using System.Reflection;
using SortingSystem;

namespace Configurator;

public static class Commands
{
    //Add all commands of this application into this and help describtion into "Messages\help.txt"
    public static void CommandDetermination(List<string> args)
    {
        string command = args[0];
        args.RemoveAt(0);
        string value = "";
        args.ForEach(x => value += " " + x);

        try
        {
            MethodInfo theMethod = typeof(Commands).GetMethod(command,
                BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase);
            object[] parameters = new object[] { };

            if (args.Count > 0)
                parameters = new object[] { value.Split("-").ToList() };

            theMethod.Invoke(null, parameters);
        }
        catch (NullReferenceException nullReferenceException)
        {
            Console.WriteLine("Unknown command or command arguments '" + command + value + "'" +
                              ", type '-h' for help");
        }
        catch (TargetInvocationException e)
        {
            Console.WriteLine("This command does not allow this syntax!");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
    
    public static void Search(List<string> commandArgs)
    {
        List<SortedItem> itemsToSearch = SortingHistory.SortedItems;
        string searchValue = commandArgs[0];
        commandArgs.RemoveAt(0);
        
        foreach (string commandArg in commandArgs)
        {
            string[] values = commandArg.ToLower().Split(" ");
            MethodInfo theMethod = typeof(SortingHistory).GetMethod(values[0],
                BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase);
            object[] parameters;

            if (Enum.TryParse(values[1].Replace("!", ""), true, out SortBy outSortBy) && theMethod.Name.FirstCharToUpper() == "Sort")
                parameters = new object[] { itemsToSearch, outSortBy, values[1][0] };
            else if (Enum.TryParse(values[1], true, out FilterBy outFilterBy) && theMethod.Name.FirstCharToUpper() == "Filter")
                parameters = new object[] { itemsToSearch, outFilterBy, values[2] };
            else
                throw new Exception("Wrong input format, type in the command 'help' to see all commands");

            theMethod.Invoke(itemsToSearch, parameters); 
        }

        itemsToSearch = SortingHistory.SearchListBy(SortingHistory.SortedItems,searchValue);
        SortingHistory.ToTable(itemsToSearch);
    }

    public static void Unused(List<string> commandArgs)
    {
        if(!int.TryParse(commandArgs[0], out int sel) || commandArgs.Count > 2)
            throw new Exception("Wrong argument, please enter months");

        SortingHistory.ToTable(SortingHistory.OlderThanXMonths(sel));
    }

    public static void Open(List<string> commandArgs)
    {
        if(!int.TryParse(commandArgs[0], out int sel) || commandArgs.Count > 2)
            throw new Exception("Wrong command argument or arguments");
        
        Process.Start(Environment.GetEnvironmentVariable("WINDIR") +
                      @"\explorer.exe",Path.GetDirectoryName(SortingHistory.LastSearchResult[sel].CurrentPath));
    }
    

    public static void Create()
    {
        Console.Write(File.ReadAllText(@"Messages\createFont.txt"));
        
        try
        {
            string name = Configurator.AddName();
            string scanDir = Configurator.AddScanDir();
            string endDir = Configurator.AddEndDir();
            List<Extension> exts = Configurator.AddExtensions(new List<Extension>());
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
                    Sorter.Sorters[sel].Extensions = Configurator.AddExtensions(Sorter.Sorters[sel].Extensions);
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
        Console.Clear();
        Console.Write(File.ReadAllText(@"Messages\help.txt") + "\n\n\n\n");
    }

    public static void Exit()
    {
        Environment.Exit(0);
    }




}