using System.Net.Http.Headers;

namespace SortingSysFinal;

public static class Logs
{
    public static List<string> Values = new List<string>();

    public static void add(string content, string SorterName)
    {
        DateTime dateTime = DateTime.Now;
        string logstr = "[" + dateTime + "] " + SorterName + ": " + content;
        Console.WriteLine(logstr);
        Values.Add(logstr);
        File.WriteAllLines("log.txt",Values);
    }
}