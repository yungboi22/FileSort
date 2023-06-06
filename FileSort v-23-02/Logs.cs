using System;
using System.Collections.Generic;
using System.IO;


namespace Sorting_Service_13_02_2023_1_0
{
    public static class Logs
    {
        public static List<string> Values = new List<string>();
        
        public static void add(string content, string SorterName)
        {
            string EnvPth = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            
            if(File.Exists(EnvPth + @"\Sorters\log.txt"))
                Values.AddRange(File.ReadAllLines(EnvPth + @"\Sorters\log.txt"));
            else
            {
                Directory.CreateDirectory(EnvPth + @"\Sorters");
                File.Create(EnvPth + @"\Sorters\log.txt");
            }
        
            DateTime dateTime = DateTime.Now;
            string logstr = "[" + dateTime + "] " + SorterName + ": " + content;
            Console.WriteLine(logstr);
            Values.Add(logstr);
        
        
            File.WriteAllLines(EnvPth + @"\Sorters\log.txt",Values);
        }
    }
    
}

