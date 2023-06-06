using System.Linq;
using System.IO;
using System;


namespace FileSort
{
    public static class Var
    {
        //Main Vars
        public static string insLoc = DefineInsLoc();
        public static string ScanDirectory = File.ReadAllText(insLoc + "ScanDirectory.ss");
        public static string EndDirectory = File.ReadAllText(insLoc + "EndDirectory.ss");
        public static string ExtensionDirectory = insLoc + "Extensions";
        public static string FileForStaticFolders = insLoc + "StaticDirectories.ss";


        //Arrays
        public static Extension[] FileExtensions;
        public static string[] StaticDirectories;
        public static string[] ExtensionFolderNames;

        public static string DefineInsLoc()
        {
            string AppName = System.AppDomain.CurrentDomain.FriendlyName;
            AppName = AppName.Replace(".exe", "");
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            return File.ReadAllText(path + @"\SortingApps\" + AppName + ".ss");
        }

        public static void Initialize()
        {
            //Define ExtensionFolderNames
            string[] ExtensionFolderPaths = Directory.GetFiles(ExtensionDirectory);
            ExtensionFolderNames = new string[ExtensionFolderPaths.Length];

            for(int i = 0; i<ExtensionFolderPaths.Length;i++)
            {
                ExtensionFolderNames[i] = Path.GetFileName(ExtensionFolderPaths[i]);
                ExtensionFolderNames[i] = Path.GetFileNameWithoutExtension(ExtensionFolderNames[i]);
            }

            //Define FileExtensions
            FileExtensions = new Extension[ExtensionFolderNames.Length];

            for(int i = 0; i<ExtensionFolderNames.Length; i++)
                FileExtensions[i] = new Extension(ExtensionFolderNames[i]);

            //Define static Directories
            var lines = File.ReadAllLines(FileForStaticFolders).Where(arg => !string.IsNullOrWhiteSpace(arg));
            File.WriteAllLines(FileForStaticFolders, lines);
            StaticDirectories = File.ReadAllLines(FileForStaticFolders);
        }

    }

    public class Extension
    {
        public string Name;
        public string[] Endings;
        private static string Location = Var.ExtensionDirectory;
        
        public Extension(string aName)
        {
            Name = aName;
            string Path = Location + @"\" + Name + ".txt"; 
            Endings = File.ReadAllLines(Path);
        }

    }
}
