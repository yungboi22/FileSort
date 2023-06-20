namespace SortingSytem_V_26_05_23;

public static class Logger
{
    private static List<string> lines;
    private static string Pth;
    
    public static void Init(string DirPath)
    {
        if (!Directory.Exists(DirPath))
            Directory.CreateDirectory(DirPath);
        
        lines = new List<string>();
        DeleteOlderThanWeek(DirPath);

        Pth = DirPath + @"\" + DateTime.Now.ToString("yyyy-dd-MMMM") + ".txt";

        if (File.Exists(Pth))
            lines = File.ReadAllLines(Pth).ToList();
        
    }
    
    
    public static void Add(string content)
    {
        string application = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        string dateTime = DateTime.Now.ToString("[dd.MM.yyyy HH:mm:ss - " + application+ "] ");
       
        
        lines.Add(dateTime + content);
        File.WriteAllLines(Pth,lines);
    }

    public static void Add(List<string> contents)
    {
        string dateTime = DateTime.Now.ToString("[dd.MM.yyyy HH:mm:ss] ");
        lines.Add(dateTime + contents);
        File.WriteAllLines(Pth,lines);
    }

    private static void DeleteOlderThanWeek(string DirPth)
    {
        string[] files = Directory.GetFiles(DirPth);
        
        foreach (string file in files)
        {
            FileInfo fi = new FileInfo(file);
            if (fi.CreationTime < DateTime.Now.AddDays(-7))
                fi.Delete();
        }
    }
    
    
    
}