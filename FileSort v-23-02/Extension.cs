using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace Sorting_Service_13_02_2023_1_0
{
    public class Extension
    {
        public string Name { get; set; }
        public List<string> Endings { get; set; }
        
        public Extension()
        {
            Name = "undefined";
            Endings = new List<string>();
        }
        
        /// <summary>
        /// Creates an extension-group
        /// </summary>
        /// <param name="aName">Name of the group (exp. Audio)</param>
        /// <param name="aEndings">String array of fileextensions (exp. '.mp3','.wav')</param>
        /// <returns></returns>
        public Extension(string aName, string[] aEndings)
        {
            Name = aName;
            Endings = aEndings.ToList();
        }
        
        /// <summary>
        /// Creates an extension-group
        /// </summary>
        /// <param name="aName">Name of the group (exp. Audio)</param>
        /// <param name="aPath">Filepath of a textfile with fileextensions (exp. '.mp3','.wav')</param>
        /// <returns></returns>
        public Extension(string aName ,string aPath)
        {
            Name = aName;

            List<string> strEndings = File.ReadAllLines(aPath).ToList();
            Endings = new List<string>();
            strEndings.ForEach(x => addEnding(x));
        }
        
        /// <summary>
        /// Adds an ending to the extension-group and checkes if it's valid. If its not it will
        /// throw an exception
        /// </summary>
        /// <param name="Ending">fileextension (exp. '.mp3','.wav')</param>
        /// <returns></returns>
        public void addEnding(string Ending)
        {
            if (!Ending.StartsWith('.'))
                throw new Exception("Error: Ending has to start with '.' !");
            
            if (!Ending.Replace(".","").All(Char.IsLetterOrDigit))
                throw new Exception("Error: In Ending: " + Ending + " in " + Name + " only alphabetic characters are allowed in an ending!");
            
            var match = Endings.FirstOrDefault(stringToCheck => stringToCheck.Contains(Ending));

            if (match != null)
                throw new Exception("Error: " + match +" already exists in " + Name);
                
            Endings.Add(Ending);
        }

        /// <summary>
        /// Removes an ending from the extension-group 
        /// </summary>
        /// <param name="Ending">fileextension (exp. '.mp3','.wav')</param>
        /// <returns></returns>
        public void removeEnding(string Ending)
        {
            Endings.Remove(Ending);
        }

        /// <summary>
        /// Converts the extension-group to a string
        /// </summary>
        /// <returns>string in format (Name) + {Extensions}</returns>
        public new string ToString()
        {
            string outp = Name + " {";
            Endings.ForEach(x => outp+= "'" + x + "'" + ",");
            outp = outp.Remove(outp.Length-1) + "}";

            return outp;
        }
        
        /// <summary>
        /// Opens the Editor-mode for an extension in a console-application. In this mode
        /// you can edit the extension group and remove endings or add new endings.
        /// </summary>
        /// <param name="extension">extension-group</param>
        /// <param name="ExtName">Name of the extensions-group</param>
        /// <returns>Edited extension</returns>
        public static Extension ExtEditMode(Extension extension,string ExtName)
        {
            List<string> inputs = extension.Endings;

            string inp = "";
                
            Console.Clear();

            do
            {
                Console.WriteLine(ExtName);
                inputs = ListValidInputs(inputs);
                inp = Console.ReadLine().ToLower().Trim();

                string number = inp.Replace("rm(", "").Replace(")", "");
                bool isIntString = number.All(char.IsDigit);
                var output = Regex.Replace(inp, @"[\d-]", string.Empty);

                if (inp != "exit" && !output.Contains("rm(") && !isIntString)
                {
                    inputs.Add(inp);
                }
                    

                if (output == "rm()" && isIntString)
                {
                    inputs.RemoveAt(Convert.ToInt32(number));
                }

                Console.Clear();
                
            } while (inp != "exit");

            return new Extension(ExtName,inputs.ToArray());
        }
        
        /// <summary>
        /// Checks which method is used to input extensions. There 3 methods:
        /// 1: is to use the 'ExtEditMode'
        /// 2: is to get extensions from textfile
        /// 3: is to get extensions from a directory with textfiles
        /// </summary>
        /// <returns>List of extensions</returns>
        public static List<Extension> GetExtByMode()
        {
            List<Extension> Ext = new List<Extension>();
            
            Console.WriteLine("(Extensionname + extensions) or (Textfile with Extensions) or (Directory with Extensiongropus). Type -h for help");
            
            
            string Inp = Console.ReadLine().Replace('"',' ').Trim();

            try
            {
                if (Inp.Trim() == "")
                {
                    throw new Exception("Extension-name cannot be empty!");
                }
                
                if (Inp== "-h")
                {
                    ExtensionCreationHelp();
                    GetExtByMode();
                    return Ext;
                }
            
                if(File.Exists(Inp))
                {
                    string fileName = Path.GetFileNameWithoutExtension(Inp);
                    Ext.Add(new Extension(fileName,Inp));
                    return Ext;
                }

                if (Directory.Exists(Inp))
                { 
                    Ext.AddRange(loadExtensions(Inp));
                    return Ext;
                }

                Ext.Add(ExtEditMode(new Extension(), Inp));
                return Ext;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n");
                return GetExtByMode();
            }
            
        }
        
        /// <summary>
        /// Prints the 'help' solution for adding extensions in the setup. It's accsesed by the
        /// GetExtByMode-Method by the input '-h'
        /// </summary>
        /// <returns></returns>
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
        
        /// <summary>
        /// This method deletes all invalid endings in a List
        /// </summary>
        /// <param name="inp">List with endings</param>
        /// <returns>Only valid endings</returns>
        private static List<string> ListValidInputs(List<string> inp)
        {
            Extension testExt = new Extension("",new string[]{});

            for (int i = 0; i < inp.Count; i++)
            {
                try
                {
                    testExt.addEnding(inp[i]);
                }
                catch (Exception e)
                {
                    inp.Remove(inp[i]);
                    continue;
                }
                
                Console.WriteLine(inp[i]);
            }

            return inp;
        }
        
        /// <summary>
        /// Loads extension groups from a directoryp-path which should contain textfiles with endings
        /// </summary>
        /// <param name="ExtDir">Path of directory with extension-groups saved as textfiles</param>
        /// <returns>List of Extension-groups</returns>
        public static List<Extension> loadExtensions(string ExtDir)
        {
            string[] extensionPaths = Directory.GetFiles(ExtDir);
            List<Extension> extensions = new List<Extension>();

            for (int i = 0; i < extensionPaths.Length; i++)
            {
                string Name = Path.GetFileNameWithoutExtension(extensionPaths[i]);
                extensions.Add(new Extension(Name,extensionPaths[i]));
            }

            return extensions;
        }

        /// <summary>
        /// Adds the default extension groups 'Ordner' and 'Rest'. They dont need any endings so they are empty.
        /// They are needed to sort directories and to sort files which have no destination
        /// </summary>
        /// <returns>List of Extension-groups with 'Rest' and 'Ordner'</returns>
        public static List<Extension> AddDefaultValues(List<Extension> extensions)
        {
            List<string> ExtNames = new List<string>();

            for (int i = 0; i < extensions.Count; i++)
                ExtNames.Add(extensions[i].Name);
            
            if(ExtNames.FirstOrDefault(ExtNames => ExtNames.Contains("Ordner")) == null)
                extensions.Add(new Extension("Ordner", new string[]{}));
            
            if(ExtNames.FirstOrDefault(ExtNames => ExtNames.Contains("Rest")) == null)
                extensions.Add(new Extension("Rest", new string[]{}));

            return extensions;
        }
    }
    
}

