using System.Linq;
using System.IO;


namespace FileSort
{
    public static class Var
    {
        //Main Vars
        public static string ScanDirectory = @"D:\All";
        public static string EndDirectory = @"D:\All\Müll";
        public static string ExtensionDirectory = @"D:\All\Random\Don't delete this folder\Extensions";
        public static string FileForStaticFolders = @"D:\All\Random\Don't delete this folder\Info\StaticFolders.txt";

        //Arrays
        public static Extension[] FileExtensions;
        public static string[] StaticDirectories;
        public static string[] ExtensionFolderNames;

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
