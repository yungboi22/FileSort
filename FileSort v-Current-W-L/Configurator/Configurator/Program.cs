

using System.Diagnostics;
using System.Net.Mime;

namespace Configurator
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.Init("Logs");
            Sorter.Sorters = new List<Sorter>();

            if (File.Exists("Sorters.json"))
            {
                string sortSysPth = File.ReadAllText("Sorters.json").Trim();
                
                if(sortSysPth != "")
                    Sorter.Sorters = Sorter.loadSorters(sortSysPth); 
            }

            try
            {
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
            bool exit = false;
            
            while(!exit)
            {
                int r = ConsoleQueries.OptionSelectQuery("What do you want to do ?",
                    new[] { "Create", "Edit", "Delete","Exit\n" });

                switch (r)
                {
                    case 1:
                        Create();
                        break;
                    case 2:
                        Edit();
                        break;
                    case 3:
                        Delete();
                        break;
                    case 4:
                        exit = true;
                        break;
                }
            }
            
            Sorter.saveSorters(Sorter.Sorters,"Sorters.json");
            FileSort.Start();
        }
        
        public static void Create()
        {
            try
            {
                string name = ConsoleQueries.StringQuery("How do you want to call your new sorter?",true,true);
                string scanDir = ConsoleQueries.DirectoryPthQuery("Please type in the path of the directory you want to scan");
                string endDir = ConsoleQueries.DirectoryPthQuery("Please type in the path of the directory where you want to sort your items");
                
                List<Extension> exts = AddExtensionsMenu();
                int[] selectedItems = ConsoleQueries.OptionSelectQueryMultiple(
                    "Select the directories which should not be affected by the sort", Utils.getItems(scanDir).ToArray());
                string[] staticDirs = Utils.selectItems(Utils.getItems(scanDir),selectedItems).ToArray();
                bool deleteFromDesktop = ConsoleQueries.YesNoQuery("Do you want to delete your items from desktop? (Y/N)");
                bool sortByYearMonth = ConsoleQueries.YesNoQuery("Do you want to sort by year and month? (Y/N)" );
                
                Sorter.Sorters.Add(new Sorter(name,scanDir,endDir,exts,staticDirs,deleteFromDesktop,sortByYearMonth));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
        public static void Edit()
        {
            if (Sorter.Sorters.Count == 0)
                return;
            
            
            List<string> names = new List<string>();
            Sorter.Sorters.ForEach(x => names.Add(x.Name));
            int sel = ConsoleQueries.OptionSelectQuery("Select a Sorter",names.ToArray()) - 1;
            bool exit = false;

            do
            {
                int sel2 = ConsoleQueries.OptionSelectQuery("Select an option",
                    File.ReadAllLines("Messages/editOptions.txt"));

                switch (sel2)
                {
                    case 1:
                        string tmp = ConsoleQueries.StringQuery("Enter the new name:", true, true);
                        Sorter.Sorters[sel].Name = tmp;
                        break;
                    case 2:
                        tmp = ConsoleQueries.DirectoryPthQuery("Enter the new scan-directory");
                        Sorter.Sorters[sel].ScanDir = tmp;
                        break;
                    case 3:
                        tmp = ConsoleQueries.DirectoryPthQuery("Enter the new scan-directory");
                        Sorter.Sorters[sel].EndDir = tmp;
                        break;
                    case 4:
                        int sel3 = ConsoleQueries.OptionSelectQuery("What do you want to do?",
                            new string[] { "Delete an extension", "Create a new extension" });
                        if (sel3 == 1)
                            Sorter.Sorters[sel].Extensions = DeleteExtensionsMenu(Sorter.Sorters[sel].Extensions);
                        else
                            Sorter.Sorters[sel].Extensions.AddRange(AddExtensionsMenu());
                        break;
                    case 5:
                        tmp = Sorter.Sorters[sel].ScanDir;
                        int[] selectedItems = ConsoleQueries.OptionSelectQueryMultiple(
                            "Select the directories which should not be affected by the sort", Utils.getItems(tmp).ToArray());
                        Sorter.Sorters[sel].StaticDirs = Utils.selectItems(Utils.getItems(tmp),selectedItems).ToArray();
                        break;
                    case 6:
                        Sorter.Sorters[sel].DeleteFromDesktop = ConsoleQueries.YesNoQuery("Do you want to delete items from desktop? (y/n)");
                        break;
                    case 7:
                        string pth = ConsoleQueries.DirectoryPthQuery("Where do you want to save your created sortingsystems? (Directory)");
                        pth += "/Systems.json";
                        File.WriteAllText("SaveLoc",pth);
                        break;
                    case 8:
                        exit = true;
                        break;
                }
            } while (!exit);
            
        }

        public static void Delete()
        {
            if (Sorter.Sorters.Count == 0)
                return;
            
            List<string> names = new List<string>();
            Sorter.Sorters.ForEach(x => names.Add(x.Name));
            int sel = ConsoleQueries.OptionSelectQuery("Select the sorter you want to delete",names.ToArray()) - 1;
            
            Sorter.Sorters.RemoveAt(sel);
        }
        
        public static List<Extension> DeleteExtensionsMenu(List<Extension> extensions)
        {
            List<string> names = new List<string>();
            extensions.ForEach(x => names.Add(x.Name));
            names.Remove("Ordner");
            names.Remove("Rest");
            
            names.ForEach(x => Console.WriteLine(x));
            
            int[] selItems = ConsoleQueries.OptionSelectQueryMultiple("Select all extensions you want to delete (Exp: 1,2)",names.ToArray());

            foreach (int selItem in selItems)
            {
                Console.WriteLine("Removing " + names[selItem]);
                extensions.RemoveAt(selItem);
            }
            
            return extensions;
        }
        
        public static List<Extension> AddExtensionsMenu()
        {
            List<Extension> extensions = new List<Extension>();
            bool exit = false;
            
            while (!exit)
            {
                int selection = ConsoleQueries.OptionSelectQuery("Please add extensions, select an option:",
                    File.ReadAllLines("Messages/addExtensionMenu.txt"));
                
                switch (selection)
                {
                    case 1:
                        string pth = ConsoleQueries.FilePthQuery("Enter the json-path:");
                        extensions.Add(new Extension(Path.GetFileNameWithoutExtension(pth),pth));
                        break;
                    case 2:
                        pth = ConsoleQueries.FilePthQuery("Enter the textfile-path:");
                        extensions.Add(new Extension(Path.GetFileNameWithoutExtension(pth),pth));
                        break;
                    case 3:
                        pth = ConsoleQueries.FilePthQuery("Enter the json-path:");
                        extensions.AddRange(Extension.LoadListFromJson(pth));
                        break;
                    case 4:
                        pth = ConsoleQueries.DirectoryPthQuery("Enter the directory-path:");
                        extensions.AddRange(Extension.LoadListFromDirectory(pth));
                        break;
                    case 5:
                        extensions.AddRange(Extension.LoadListFromConsole());
                        break;
                    case 6:
                        Console.WriteLine(File.ReadAllText("Messages/addExtensionMenuHelp.txt"));
                        Console.WriteLine();
                        break;
                    case 7:
                        if (!extensions.Any())
                            Console.WriteLine("You have to add at least one extension!");
                        else
                            exit = true;
                        break;
                }
            }

            return extensions;
        }
    }
}