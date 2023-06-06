using System.Globalization;
using System.Text.RegularExpressions;
using System.IO;

namespace SortingSysFinal;

public static class IOMethod
{
    private static bool isDirectory(string Pth)
    {
        FileAttributes attr = File.GetAttributes(Pth);
        
        if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            return true;
        
        return false;
    }
    
    public static bool NewItemLoc(string ScanPth, string[] StaticDirs)
    {
        string[] Dirs = Directory.GetDirectories(ScanPth);
        bool isEqual = Enumerable.SequenceEqual(Dirs, StaticDirs);
        
        if (isEqual && Directory.GetFiles(ScanPth).Length == 0)
            return false;

        return true;
    }

    public static string[] GetNewItems(string ScanPth, string[] StaticDir)
    {
        List<string> NewItems = new List<string>();
        NewItems.AddRange(Directory.GetDirectories(ScanPth));
        NewItems.AddRange(Directory.GetFiles(ScanPth));

        foreach (string Dir in StaticDir)
            NewItems.Remove(Dir);
        
        return NewItems.ToArray();
    }

    public static void RemoveFromDesktop(string[] Items)
    {
        foreach (string item in Items)
        {
            string pth = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (isDirectory(item))
            {
                if (Directory.Exists(pth + @"\"  + new DirectoryInfo(item).Name))
                    Directory.Delete(pth + @"\"  + new DirectoryInfo(item).Name, true);
            }
            else
            {
                if (File.Exists(pth + @"\"  + Path.GetFileName(item)))
                    File.Delete(pth + @"\"  + Path.GetFileName(item));
            }
        }
    }

    public static void CreateEndFolders(List<Extension> extensions, string EndDir)
    {
        foreach (Extension exten in extensions)
            if (!Directory.Exists(EndDir + @"\" + exten.Name))
                Directory.CreateDirectory(EndDir + @"\" + exten.Name);
    }
   
    public static void MoveItem(string ItemPth,string EndDir,bool SortByDate, Sorter sorter)
    {
        string ItemName = Path.GetFileNameWithoutExtension(ItemPth);
        string ExtensionDir = @"\" + GetExtensionDir(ItemPth,sorter.Extensions);
        
        string DatePath = "";
        if(SortByDate)
            DatePath = GetDatePath(ItemPth,EndDir + @"\" + ExtensionDir);
        
        string TmpPath = EndDir + @"\" + ExtensionDir + @"\" + DatePath;
        string NewItemName = CheckName(ItemName,GetCompItems(TmpPath));
        string destinationPath = TmpPath + @"\" + NewItemName + Path.GetExtension(ItemPth);
        
        Logs.add("Moved file from " +ItemPth + " to " +destinationPath,sorter.Name);
        Directory.Move(ItemPth,destinationPath); 
    }
    
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

   public static string GetDatePath(string ItemPth, string Loc)
   {
       DateTime modification = File.GetLastWriteTime(ItemPth);
       CultureInfo ci = new CultureInfo("de-DE");
       
       string Year = @"\" + modification.Year;
       string Month = @"\" + modification.ToString("MMMM", ci);

       if (!Directory.Exists(Loc + @"\" + Year))
           Directory.CreateDirectory(Loc + @"\" + Year);

       if (!Directory.Exists(Loc + @"\" + Year + @"\" + Month))
           Directory.CreateDirectory(Loc + @"\" + Year + @"\" + Month);
       
       return Year + @"\" + Month;
   }
   
   private static List<string> GetCompItems(string Pth)
   {
        List<string> CompItems = new List<string>();
        CompItems.AddRange( Directory.GetDirectories(Pth));
        CompItems.AddRange( Directory.GetFiles(Pth));

        for (int i = 0; i < CompItems.Count; i++)
            CompItems[i] = Path.GetFileNameWithoutExtension(CompItems[i]);
        
        return CompItems;
   }
   
   public static string CheckName(string ItemName, List<string> CompItems)
   {
       foreach (string CompItem in CompItems)
       {
           if (CompItem == ItemName)
           {
               if (BracketNumberFormat(ItemName))
                   ItemName = ReplaceNum(ItemName, GetFileNameNum(ItemName)+1);
               else
                   ItemName+= " (" + 2 + ")";
               
               ItemName = CheckName(ItemName, CompItems);
           }
       }
        
       return ItemName;
   }

   private static int GetFileNameNum(string input)
   {
       Regex rgx = new Regex("[^0-9 -]");
       input = rgx.Replace(input, "");
        
       return Convert.ToInt32(input);
   }
    
    private static bool BracketNumberFormat(string input)
    {
        var output = Regex.Replace(input, @"[\d-]", string.Empty);
        if (output.Contains("()") && input.Any(char.IsDigit))
            return true;
            
        return false;
    }
    
    private static string ReplaceNum(string input,int NewNum)
    {
        string output = Regex.Replace(input, @"[\d-]", string.Empty);
        output = output.Substring(0, output.IndexOf("()"));
        output += "(" + NewNum+ ")" ;

        return output;
    }
}