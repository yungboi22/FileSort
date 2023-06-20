using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.RegularExpressions;


namespace SortingSystem
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
        
        public Extension(string aName, string[] aEndings)
        {
            Name = aName;
            Endings = new List<string>();
            
            foreach (string ending in aEndings)
                addEnding(ending);
            
        }

        public Extension(string aName ,string aPath)
        {
            Name = aName;

            List<string> strEndings = File.ReadAllLines(aPath).ToList();
            Endings = new List<string>();
            strEndings.ForEach(x => addEnding(x));
        }
        
        /// <summary>
        /// Constructor using Json-file
        /// </summary>
        /// <param name="aPath"> path to json-file (/home/Audio.json)</param>
        public Extension(string aPath)
        {
            string obj = File.ReadAllText(aPath);
            var options = new JsonSerializerOptions { WriteIndented = true }; 
            Extension extension = JsonSerializer.Deserialize<Extension>(obj,options)!;

            Name = extension.Name;
            Endings = new List<string>();
            extension.Endings.ForEach(x => addEnding(x));
        }
        
        /// <summary>
        /// Adds an ending to the extension-group and checkes if it's valid. If its not it will
        /// throw an exception
        /// </summary>
        /// <param name="Ending">fileextension (exp. '.mp3','.wav')</param>
        /// <returns></returns>
        public void addEnding(string Ending)
        {
            if(validEnding(Ending))
                Endings.Add(Ending);
            else
                throw new Exception("Error check format: .<alphabetic chars> and check if ending already exists!");
        }

        public bool validEnding(string Ending)
        {
            if (!Ending.StartsWith('.'))
                return false;

            if (!Ending.Replace(".", "").All(Char.IsLetterOrDigit))
                return false;
            
            var match = Endings.FirstOrDefault(stringToCheck => stringToCheck.Contains(Ending));

            if (match != null)
                return false;

            return true;
        }

        public void removeEnding(string Ending)
        {
            Endings.Remove(Ending);
        }
        
        public override string ToString()
        {
            string outp = Name + " {";
            Endings.ForEach(x => outp+= "'" + x + "'" + ",");
            outp = outp.Remove(outp.Length-1) + "}";

            return outp;
        }
        
        /// <summary>
        /// Checks which method is used to input extensions. There 3 methods:
        /// 1: is to use the 'ExtEditMode'
        /// 2: is to get extensions from textfile
        /// 3: is to get extensions from a directory with textfiles
        /// </summary>
        /// <returns>List of extensions</returns>
        
        public static List<Extension> LoadListFromJson(string Pth)
        {
            string obj = File.ReadAllText(Pth);
            var options = new JsonSerializerOptions { WriteIndented = true }; 
            return JsonSerializer.Deserialize<List<Extension>>(obj,options)!;
        }

        public static List<Extension> LoadListFromDirectory(string Pth)
        {
            string[] extPthFiles = Directory.GetFiles(Pth);
            List<Extension> extensions = new List<Extension>();

            foreach (string extPthFile in extPthFiles)
                extensions.Add(new Extension(Path.GetFileNameWithoutExtension(extPthFile),extPthFile));

            return extensions;
        }
        
        public static List<Extension> LoadListFromConsole()
        {
            List<Extension> extensions = new List<Extension>();
            
            do
            {
                try
                {
                    string name = ConsoleQueries.StringQuery("Name (!q to quit): ",false,false);
                    if (name == "!q") {Console.Write("\n"); break;}
                    string[] endings = ConsoleQueries.StringQuery("Endings (.xx .xz): ",false,false).Split(" ");
                    extensions.Add(new Extension(name,endings));
                    Console.WriteLine(name + "-extension was successfully added to the extensionlist! \n");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message + "\n");
                }
            } while (true);
            
            return extensions;
        }
        
        public static void SaveListToJson(List<Extension> extensions, string Pth)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string obj = JsonSerializer.Serialize(extensions,options);
            File.WriteAllText(Pth,obj);
        }
        
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

