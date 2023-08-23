using SortingSystem;

namespace Configurator;

public static class Configurator
{

    public static string AddName()
    {
        string name = ConsoleUtils.StringQuery("\nHow do you want to call your new sorter?", true);

        foreach (Sorter sorter in Sorter.Sorters)
        {
            if (sorter.Name == name)
            {
                Console.WriteLine("\nA sorter with this name already exists, please try again.");
                return AddName();
            }
        }

        return name;
    }

    public static string AddScanDir()
    {
        string scanDir =
            ConsoleUtils.DirectoryPthQuery("\nPlease type in the path of the directory you want to scan");

        foreach (Sorter sorter in Sorter.Sorters)
        {
            if (sorter.ScanDir == scanDir)
            {
                Console.WriteLine(
                    "\n" + sorter.Name + " is already scanning this directory, please try something else.");
                return AddScanDir();
            }
        }

        return scanDir;
    }

    public static string AddEndDir()
    {
        string endDir =
            ConsoleUtils.DirectoryPthQuery(
                "\nPlease type in the path of the directory where you want to sort your items");

        foreach (Sorter sorter in Sorter.Sorters)
        {
            if (sorter.EndDir == endDir)
            {
                if (ConsoleUtils.YesNoQuery("\n" + sorter.Name +
                                            " is already using this as an end-directory, do you want to still use it? (y/n)"))
                    return endDir;

                return AddEndDir();
            }
        }

        return endDir;
    }

    public static List<Extension> AddExtensions(List<Extension> extensions)
    {
        extensions = Extension.RemoveDefaultValues(extensions);
        bool exit = false;

        while (!exit)
        {
            try
            {
                Console.WriteLine();
                int selection = ConsoleUtils.OptionSelectQuery("Select an option to add an extension:",
                    File.ReadAllLines("Messages/addExtensionMenu.txt")) + 1;
                Console.WriteLine();

                switch (selection)
                {
                    case 1:
                        string pth = ConsoleUtils.JsonFileQuery("Enter the json-path (!q to exit):", false);
                        if (pth == "!q") break;
                        extensions = Extension.Add(extensions, new Extension(pth));
                        break;
                    case 2:
                        pth = ConsoleUtils.FilePthQuery("Enter the textfile-path (!q to exit):");
                        if (pth == "!q") break;
                        extensions = Extension.Add(extensions,
                            new Extension(Path.GetFileNameWithoutExtension(pth), pth));
                        break;
                    case 3:
                        pth = ConsoleUtils.JsonFileQuery("Enter the json-path (!q to exit):", true);
                        if (pth == "!q") break;
                        extensions = Extension.AddRange(extensions, Extension.LoadListFromJson(pth));
                        break;
                    case 4:
                        pth = ConsoleUtils.DirectoryPthQuery("Enter the directory-path (!q to exit):");
                        if (pth == "!q") break;
                        extensions = Extension.AddRange(extensions, Extension.LoadListFromDirectory(pth));
                        break;
                    case 5:
                        extensions = Extension.LoadListFromConsole(extensions);
                        break;
                    case 6:
                        if (!extensions.Any())
                            Console.WriteLine("You have to add at least one extension to use this!");
                        else
                            extensions = DeleteMenu(extensions);
                        break;
                    case 7:
                        Console.WriteLine(File.ReadAllText("Messages/addExtensionMenuHelp.txt"));
                        Console.WriteLine();
                        break;
                    case 8:
                        if (!extensions.Any())
                            Console.WriteLine("Your sorter should have at least one extension !");
                        else
                            exit = true;
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        return Extension.AddDefaultValues(extensions);
    }

    public static bool AddDeleteFromDesktop()
    {
        return ConsoleUtils.YesNoQuery("\nDo you want to delete items from desktop? (y/n)");
    }

    public static List<Extension> DeleteMenu(List<Extension> extensions)
    {
        List<string> stringExts = new List<string>();
        extensions.ForEach(x => stringExts.Add(x.ToStringShort(5)));
        stringExts.ForEach(x => Console.WriteLine(x));

        bool answer = ConsoleUtils.YesNoQuery("\nY: Delete an extension, N: Exit");

        if (answer)
        {
            List<int> selectedVals = ConsoleUtils.OptionSelectQueryMultiple(
                "\nWhich extensions do you want to delete",
                stringExts.ToArray()).ToList();

            for (int i = 0; i < selectedVals.Count; i++)
            {
                extensions.RemoveAt(selectedVals[i]);

                for (int j = 0; j < selectedVals.Count; j++)
                    selectedVals[j]--;
            }
        }

        return extensions;
    }

    public static string[] AddStaticDirs(string scanDir)
    {
        List<string> items = Utils.getItems(scanDir);

        if (!items.Any())
            return new string[] { };

        int[] selectedItems = ConsoleUtils.OptionSelectQueryMultiple(
            "\nSelect the directories which should not be affected by the sort", Utils.getItems(scanDir).ToArray());

        return Utils.selectItems(Utils.getItems(scanDir), selectedItems).ToArray();
    }
}