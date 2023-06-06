using System.Globalization;

namespace SortingSysFinal;

public static class Setup
{
    public static void Open()
    {
        Sorter.stopSorters(SortingSysFinal.Sorters);
        
        Console.Write("Welcome to the Sortingsystem-Setup, the application is used for sorting files and directories.\n" +
                      "This gets done by a background process that scans a location at a specified interval and then sorts\n" +
                      "new items into a new specified location and even by months and years if desired. The Setup allows you\n" +
                      "to install, update, repair or delete Sortingsystems"+
                      "\nDeveloped by -@Andi- V: 1.0"+
                      "\n\n");

        int r = ConsoleQueries.OptionSelectQuery("What do you want to do ?",new []{"Install","Update","Repair", "Delete\n"});

        switch (r)
        {
            case 1:
                Install();
                break;
            case 2:
                break;
            case 3:
                break;
            case 4: 
                break;
        }
        
        Console.WriteLine("Starting Sorters...\n");
        Sorter.saveSorters(SortingSysFinal.Sorters,"Sortingsystems.json");
        Sorter.startSorters(SortingSysFinal.Sorters);
    }
    
    public static void Install()
    {
        string Name = ConsoleQueries.NameQuery("How do you want to call your new sorter?");
        string ScanDir = ConsoleQueries.PathQuery("Please type in the path of the directory you want to scan");
        string EndDir = ConsoleQueries.PathQuery("Please type in the path of the directory where you want to sort your items");
        
        List<Extension> exts = ConsoleQueries.GetExtensionsWiConsole();
        int[] selectedItems = ConsoleQueries.OptionSelectQueryMultiple(
            "Select the directories which should not be affected by the sort", Utils.getItems(ScanDir).ToArray());
        string[] StaticDir = Utils.selectItems(Utils.getItems(ScanDir),selectedItems).ToArray();
        bool DeleteFromDesktop = ConsoleQueries.YesNoQuery("Do you want to delete your items from desktop? (Y/N)");
        bool SortByYearMonth = ConsoleQueries.YesNoQuery("Do you want to sort by year and month? (Y/N)" );
        int tick = ConsoleQueries.IntQuery("Please enter a tick rate");

        SortingSysFinal.Sorters.Add(new Sorter(Name,ScanDir,EndDir,exts,StaticDir,DeleteFromDesktop,SortByYearMonth));
    }

    
    public static void ExtensionCreationHelp()
    {
        Console.WriteLine(
            "\nThe program uses groups with file extensions for sorting." +
            "\nFor example the group 'Audio' with the extensions '.mp3','.wav' will create an " +
            "\naudio folder in the end directory, where the files will be sorted by their extensions." +
            "\nYou can define as many groups with file extensions as you want, there are 3 different options which you can use," +
            "\nit will be selected automatically by scanning your input:");
        Console.WriteLine(
            "\n1. Via the console, type in the name of the group in the first line and " +
            "\nthen an file-ending in each line. You can delete the first line you have entered" +
            "\nwith rm(1) or leave the selection with exit and create a new group.");
        Console.WriteLine(
            "\n2. Specify a file path with a text file containing the name of the group" +
            "\nand containing the extensions as lines.");
        Console.WriteLine(
            "\n3. Enter a path to a folder that contains several text files with" +
            "\nthe respective group names and the extensions.");
        Console.WriteLine("Press ENTER to leave");
        
        Console.ReadLine();
        Console.Clear();
    }
    
    
  
    
        
}