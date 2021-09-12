using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Globalization;

namespace FileSort
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x80;  // Turn on WS_EX_TOOLWINDOW
                return cp;
            }
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            this.Visible = false;
            Var.Initialize();
            Timer_Check.Start();
            timer_SortByMonths.Start();
        }

        private void Timer_Check_Tick(object sender, EventArgs e)
        {
            if (NewItemLocated())
            {
                string[] NewItems = GetNewItems();
                RemoveItemsFromDesktop(NewItems);
                CreateEndFolders();

                for (int i = 0; i < NewItems.Length; i++)
                    MoveItem(NewItems[i]);
            }
        }

        private void timer_SortByMonths_Tick(object sender, EventArgs e)
        {
            string[] ExtensionDirectories = Directory.GetDirectories(Var.EndDirectory);

            foreach(string ExtensionDir in ExtensionDirectories)
            {
                string[] Files = Directory.GetFiles(ExtensionDir);
 
                foreach (string File in Files)
                    MoveFileToDate(File);

            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        private bool NewItemLocated()
        {
            string[] SubDir = Directory.GetDirectories(Var.ScanDirectory);
            bool isEqual = Enumerable.SequenceEqual(SubDir, Var.StaticDirectories);

            bool ContainsNoFile;
            if (Directory.GetFiles(Var.ScanDirectory).Length == 0)
                ContainsNoFile = true;
            else
                ContainsNoFile = false;

            if (ContainsNoFile && isEqual)
                return false;
            else
                return true;
        } 

        private string[] GetNewItems()
        {
            string[] x = Directory.GetDirectories(Var.ScanDirectory);
            string[] y = Directory.GetFiles(Var.ScanDirectory);
   
            string[] z = new string[x.Length + y.Length];
            x.CopyTo(z, 0);
            y.CopyTo(z, x.Length);

            for(int i =0; i<Var.StaticDirectories.Length; i++)
                z = z.Where(val => val != Var.StaticDirectories[i]).ToArray();

             return z;
        }

        private void RemoveItemsFromDesktop(string[] aItems)
        {
            
            for(int i = 0; i<aItems.Length; i++)
            {
                FileAttributes attr = File.GetAttributes(aItems[i]);

                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    //Directory
                    aItems[i] = new DirectoryInfo(aItems[i]).Name;
                    if(Directory.Exists(@"C:\Users\64dre\Desktop\" + aItems[i]))
                        Directory.Delete(@"C:\Users\64dre\Desktop\" + aItems[i],true);
                }
                else
                {
                    //File
                    aItems[i] = Path.GetFileName(aItems[i]);
                    if(File.Exists(@"C:\Users\64dre\Desktop\" + aItems[i]))
                        File.Delete(@"C:\Users\64dre\Desktop\" + aItems[i]);
                }
            }
        }

        private void CreateEndFolders()
        {
            for(int i = 0; i< Var.ExtensionFolderNames.Length; i++)
            {
                string Path = Var.EndDirectory +@"\"+ Var.ExtensionFolderNames[i];
                if (!Directory.Exists(Path))
                    Directory.CreateDirectory(Path);
            } 
        }

        private void MoveItem(string ItemName)
        {
            FileAttributes attr = File.GetAttributes(Var.ScanDirectory + @"\" + ItemName);

            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                //Directory
                string NewItemName = CheckDirName(ItemName);
                string sourcePath = Var.ScanDirectory + @"\" + ItemName;
                string destinationPath = Var.EndDirectory+ @"\Ordner" + @"\" + NewItemName;
                Directory.Move(sourcePath,destinationPath);
            }
            else
            {
                //File
                string ExtensionDir = @"\" + GetExtensionDir(ItemName);
                string NewItemName = CheckFileName(ExtensionDir ,ItemName);
                string sourcePath = Var.ScanDirectory + @"\" + ItemName;
                string destinationPath = Var.EndDirectory + ExtensionDir + @"\" + NewItemName;
                Directory.Move(sourcePath, destinationPath);
            }
        }

        private string GetExtensionDir(string input)
        {
            input = Var.ScanDirectory + @"\" + input;
            string ext = Path.GetExtension(input);
            
            foreach(Extension extension in Var.FileExtensions)           
                foreach(string ending in extension.Endings)                
                    if(ending == ext)                  
                        return extension.Name;
 
            return "Rest";
        }

        private string CheckFileName(string ExtensionDir,string FileName)
        {
            string[] CompFilePaths = Directory.GetFiles(Var.EndDirectory + ExtensionDir, "*.*", SearchOption.AllDirectories);

            for (int i = 0; i < CompFilePaths.Length; i++)
                CompFilePaths[i] = Path.GetFileName(CompFilePaths[i]);
                    
            for (int i = 1; ; i++)
            {
                int counter = 0;
                foreach (string CompFile in CompFilePaths)
                {
                    if (CompFile == FileName)
                    {
                        if (BracketNumberFormat(FileName))
                        {
                            string Ext = GetExtension(FileName);
                            string NoExt = RemoveExtension(FileName);
                            NoExt = ReplaceNum(NoExt, i);
                            FileName = NoExt + Ext;
                        }
                        else
                        {
                            string Ext = GetExtension(FileName);
                            string NoExt = RemoveExtension(FileName);
                            NoExt += " (" + i.ToString() + ")";
                            FileName = NoExt + Ext;
                        }
                        break;
                    }
                    else
                    {
                        counter++;
                        if (counter == CompFilePaths.Length)
                            break;
                    }
                }

                if (counter == CompFilePaths.Length)
                    break;

            }

            return FileName;
        }

        private string CheckDirName(string DirName)
        {
            string[] CompDirPaths = Directory.GetDirectories(Var.EndDirectory + @"\Ordner");

            for(int i = 0; i<CompDirPaths.Length;i++)
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(CompDirPaths[i]);
                CompDirPaths[i] = directoryInfo.Name;
            }

            for (int i = 1; ;i++)
            {
                int counter = 0;
                foreach (string CompDir in CompDirPaths)
                {
                    if (CompDir == DirName)
                    {                  
                        if(BracketNumberFormat(DirName))
                        {
                            DirName = ReplaceNum(DirName, i);
                        }
                        else
                        {
                            DirName += " (" + i.ToString() + ")";
                        }
                        break;
                    }
                    else
                    {
                        counter++;
                        if (counter == CompDirPaths.Length)
                            break;
                    }
                }

                if (counter==CompDirPaths.Length)
                    break;
                                
            }

            return DirName;
        }

        private bool BracketNumberFormat(string input)
        {
            var output = Regex.Replace(input, @"[\d-]", string.Empty);
            if (output.Contains("()") && input.Any(char.IsDigit))
                return true;
            else
                return false;
        }

        private string ReplaceNum(string input,int NewNum)
        {
            string output = Regex.Replace(input, @"[\d-]", string.Empty);
            output = output.Substring(0, output.IndexOf("()"));
            output += "(" + NewNum.ToString() + ")" ;

            return output;
        }

        private string RemoveExtension(string input)
        {
            int dotLoc = input.LastIndexOf('.');
            string output = input.Substring(0, dotLoc);

            return output;
        }

        private string GetExtension(string input)
        {
            int dotLoc = input.LastIndexOf('.');
            string output = input.Substring(dotLoc,input.Length - dotLoc);

            return output;
        }

        private void MoveFileToDate(string path)
        {
            DateTime modification = File.GetLastWriteTime(path);
            string FileName = Path.GetFileName(path);
            string LastDirectory = Path.GetDirectoryName(path);

            string Year = @"\" + modification.Year.ToString();
            CultureInfo ci = new CultureInfo("de-DE");
            string month = @"\" + modification.ToString("MMMM", ci);

            if (!Directory.Exists(LastDirectory + Year))
                Directory.CreateDirectory(LastDirectory + Year);

            if (!Directory.Exists(LastDirectory + Year + month))
                Directory.CreateDirectory(LastDirectory + Year + month);

            File.Move(path, LastDirectory + Year + month + @"\" + FileName);

        }

    }
}
