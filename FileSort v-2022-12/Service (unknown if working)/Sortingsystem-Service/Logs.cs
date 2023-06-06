using System.Net.Http.Headers;

namespace Sortingsystem_Service;

public static class Logs
{
    public static List<string> Values = new List<string>();
    
    public static void add(string content, string SorterName)
    {
        if(File.Exists(@"Logs\log.txt"))
            Values.AddRange(File.ReadAllLines(@"Logs\log.txt"));
        else
        {
            Directory.CreateDirectory(@"Logs");
            File.Create(@"Logs\log.txt");
        }
        
        DateTime dateTime = DateTime.Now;
        string logstr = "[" + dateTime + "] " + SorterName + ": " + content;
        Console.WriteLine(logstr);
        Values.Add(logstr);
        
        
        File.WriteAllLines(@"Logs\log.txt",Values);
    }
}