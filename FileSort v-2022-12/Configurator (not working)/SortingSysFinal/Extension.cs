using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace SortingSysFinal;

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
        Endings = aEndings.ToList();
    }
    
    public Extension(string aName ,string aPath)
    {
        Name = aName;

        List<string> strEndings = File.ReadAllLines(aPath).ToList();
        Endings = new List<string>();
        strEndings.ForEach(x => addEnding(x));
    }
    
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

    public void removeEnding(string Ending)
    {
        Endings.Remove(Ending);
    }

    public new string ToString()
    {
        string outp = Name + " {";
        Endings.ForEach(x => outp+= "'" + x + "'" + ",");
        outp = outp.Remove(outp.Length-1) + "}";

        return outp;
    }
    
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
                Setup.ExtensionCreationHelp();
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