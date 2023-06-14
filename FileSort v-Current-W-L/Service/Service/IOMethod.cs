using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic.CompilerServices;

namespace SortingSytem_V_26_05_23
{
    public static class IOMethod
    {
        private static string OSTk; //Win: "\" or Linux: "/"
        
        /// <summary>
        /// Initializes tbe IOMethod static class,
        /// </summary>
        public static void Init()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                OSTk = @"\";
            else
                OSTk = "/";
        }
        
        /// <summary>
        /// Check's if a Path is a directory or a file
        /// </summary>
        /// <param name="Path">Path of a textfile or directory</param>
        /// <returns>true: when directory
        /// false: when file </returns>
        private static bool isDirectory(string Pth)
        {
            FileAttributes attr = File.GetAttributes(Pth);
            
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                return true;
            
            return false;
        }
    
        /// <summary>
        /// Check's if a new item is located (item = files and directories)
        /// </summary>
        /// <param name="ScanPth">Path to scan for new items</param>
        /// /// <param name="StaticDirs">Items that can be ignored</param>
        /// <returns>true: when new items are located</returns>
        public static bool NewItemLoc(string ScanPth, string[] StaticDirs)
        {
            string[] Dirs = Directory.GetDirectories(ScanPth);
            bool isEqual = Enumerable.SequenceEqual(Dirs, StaticDirs);
            
            if (isEqual && Directory.GetFiles(ScanPth).Length == 0)
                return false;

            return true;
        }
        
        /// <summary>
        /// Gets new items (item = files and directories)
        /// </summary>
        /// <param name="ScanPth">Path to scan for new items</param>
        /// /// <param name="StaticDirs">Items that can be ignored</param>
        /// <returns>array with new itempaths</returns>
        public static string[] GetNewItems(string ScanPth, string[] StaticDir, string EndDir)
        {
            List<string> NewItems = new List<string>();
            NewItems.AddRange(Directory.GetDirectories(ScanPth));
            NewItems.AddRange(Directory.GetFiles(ScanPth));
            NewItems.Remove(EndDir);
            
            
            
            foreach (string Dir in StaticDir)
                NewItems.Remove(Dir);
            
            return NewItems.ToArray();
        }

        /// <summary>
        /// Removes items from desktop if they exist
        /// </summary>
        /// <param name="Items">files and directories</param>
        public static void RemoveFromDesktop(string[] Items)
        {
            foreach (string item in Items)
            {
                string pth = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                if (isDirectory(item))
                {
                    if (Directory.Exists(pth + OSTk  + new DirectoryInfo(item).Name))
                        Directory.Delete(pth + OSTk  + new DirectoryInfo(item).Name, true);
                }
                else
                {
                    if (File.Exists(pth + OSTk  + Path.GetFileName(item)))
                        File.Delete(pth + OSTk  + Path.GetFileName(item));
                }
            }
        }

        /// <summary>
        /// Creates the endfolders where the system sorts your items to. The folders
        /// are based on your defined extensions and uses the names of the extensions.
        /// </summary>
        /// <param name="extensions">Your extension-groups</param>
        /// /// <param name="EndDir">Enddirecotry</param>
        public static void CreateEndFolders(List<Extension> extensions, string EndDir)
        {
            foreach (Extension exten in extensions)
                if (!Directory.Exists(EndDir + OSTk + exten.Name))
                    Directory.CreateDirectory(EndDir + OSTk + exten.Name);
        }
       
        /// <summary>
        /// Moves a item from ScanDir to Enddir + DatePath if true
        /// </summary>
        /// <param name="SortByDate">When true when the file gets into a folder with the current month and year</param>
        public static void MoveItem(string ItemPth,string EndDir,bool SortByDate, Sorter sorter)
        {
            string ItemName = Path.GetFileNameWithoutExtension(ItemPth);
            string ExtensionDir = OSTk + GetExtensionDir(ItemPth,sorter.Extensions);
            
            string DatePath = "";
            if(SortByDate)
                DatePath = GetDatePath(ItemPth,EndDir + OSTk + ExtensionDir);
            
            string TmpPath = EndDir + OSTk + ExtensionDir + OSTk + DatePath;
            string NewItemName = Utils.CheckName(ItemName,GetCompItems(TmpPath));
            string destinationPath = TmpPath + OSTk + NewItemName + Path.GetExtension(ItemPth);
            
            
            Logger.Add("Moving " + ItemPth + " to " + destinationPath);
            Directory.Move(ItemPth, destinationPath);
        }
        
        /// <summary>
        /// Gets the final directory where the items has to be sorted in based on its fileextension
        /// </summary>
        /// <returns>Path of directory</returns>
        private static string GetExtensionDir(string ItemPth,List<Extension> Extensions)
        {
            string ext = Path.GetExtension(ItemPth);
           
            if (isDirectory(ItemPth))
                return "Ordner";
           
            foreach (Extension extension in Extensions)
            {
                foreach (string ending in extension.Endings)
                {
                  
                    if(ending == ext)                  
                        return extension.Name;  
                }
            }          
           
            return "Rest";
        }

        /// <summary>
        /// Gets the path of the item based on its month and year
        /// </summary>
        /// <returns>New path with enddir + extdir + datedir</returns>
        public static string GetDatePath(string ItemPth, string Loc)
        {
            DateTime modification = File.GetLastWriteTime(ItemPth);
            CultureInfo ci = new CultureInfo("de-DE");
           
            string Year = OSTk + modification.Year;
            string Month = OSTk + modification.ToString("MMMM", ci);

            if (!Directory.Exists(Loc + OSTk + Year))
                Directory.CreateDirectory(Loc + OSTk + Year);

            if (!Directory.Exists(Loc + OSTk + Year + OSTk + Month))
                Directory.CreateDirectory(Loc + OSTk + Year + OSTk + Month);
           
            return Year + OSTk + Month;
        }
       
        /// <summary>
        /// Gets compare items 
        /// </summary>
        /// <returns>Paths of compare-items</returns>
        private static List<string> GetCompItems(string Pth)
        {
            List<string> CompItems = new List<string>();
            CompItems.AddRange( Directory.GetDirectories(Pth));
            CompItems.AddRange( Directory.GetFiles(Pth));

            for (int i = 0; i < CompItems.Count; i++)
                CompItems[i] = Path.GetFileNameWithoutExtension(CompItems[i]);
            
            return CompItems;
        }
       

    }
    
}

