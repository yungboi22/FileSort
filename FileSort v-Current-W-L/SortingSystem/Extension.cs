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
        
        public string ToStringShort(int count)
        {
            List<string> trimedEndings = Endings.Take(count).ToList();

            string outp = Name + " {";
            trimedEndings.ForEach(x => outp+= "'" + x + "'" + ",");
            outp += "...}";

            return outp;
        }

        public static List<Extension> Add(List<Extension> extensionList, Extension extensionToAdd)
        {
            foreach (Extension ext in extensionList)
            {
                if (ext.Name.ToLower() == extensionToAdd.Name.ToLower())
                    throw new Exception(ext.Name + " already exists!");
            }
            
            extensionList.Add(extensionToAdd);

            return extensionList;
        }

        public static List<Extension> AddRange(List<Extension> extensionList, List<Extension> extensionListToAdd)
        {
            foreach (Extension ext in extensionList)
            {
                foreach (Extension ext2 in extensionListToAdd)
                {
                    if (ext.Name.ToLower() == ext2.Name.ToLower())
                        throw new Exception(ext.Name + " already exists!");
                }
            }
            
            extensionList.AddRange(extensionListToAdd);

            return extensionList;
        }
        
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
        
        public static List<Extension> LoadListFromConsole(List<Extension> extensions)
        {
            do
            {
                try
                {
                    string name = ConsoleUtils.StringQuery("Name (!q to quit): ",false).Trim();
                    if (name == "!q") {Console.Write("\n"); break;}
                    string[] endings = ConsoleUtils.StringQuery("Endings (.xx .xz): ",false).Split(" ");
                    extensions = Add(extensions,new Extension(name,endings));
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
        
        public static List<Extension> RemoveDefaultValues(List<Extension> extensions)
        {
            List<string> ExtNames = new List<string>();
            extensions.ForEach(x => ExtNames.Add(x.Name));

            for (var i = 0; i < ExtNames.Count; i++)
            {
                if (ExtNames[i] == "Ordner" || ExtNames[i] == "Rest")
                {
                    ExtNames.RemoveAt(i);
                    extensions.Remove(extensions[i]);
                    i--;
                }
            }
            
            return extensions;
        }
    }
    
}

