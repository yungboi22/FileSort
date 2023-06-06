namespace SortingSysFinal;

public static class ConsoleQueries
{
    public static string NameQuery(string Query)
    {
        Console.WriteLine(Query);
        string output = Console.ReadLine().Trim('"').Trim(' ');
        Console.Clear();
        return output;
    }
    
    public static int IntQuery(string Query)
    {
        Console.WriteLine(Query);
        string output = Console.ReadLine().Trim('"').Trim(' ');

        int n;
        if (!int.TryParse(output, out n))
        {
            Console.WriteLine("Please enter an integer");
            return IntQuery(Query); 
        }
           

        Console.Clear();
        return n;
    }

    public static string PathQuery(string Query)
    {
        string output = NameQuery(Query);

        if (Directory.Exists(output))
            return output;
        
        try
        {
            Directory.CreateDirectory(output);
            Console.Clear();
            return output;
        }
        catch (IOException)
        {
            Console.WriteLine("Directory does not exist and could not be created!\n");
            return PathQuery(Query);
        }
    }
    
    public static int OptionSelectQuery(string Query, string[] QueryValues)
    {
        Console.WriteLine(Query);
            
        for(int i = 0; i < QueryValues.Length; i++)
            Console.WriteLine(Convert.ToString(i+1) + ": " + QueryValues[i]);
            
        int n;

        if (!int.TryParse(Console.ReadLine().Trim('"').Trim(' '), out n))
            return OptionSelectQuery(Query, QueryValues);
            
        if (n >= QueryValues.Length+1 || n <= 0) 
        { 
            Console.WriteLine("This Option does not exist"); 
            return OptionSelectQuery(Query, QueryValues);
        }            
            
        Console.Clear();

        return n;
            
    }
    
    public static int[] OptionSelectQueryMultiple(string Query, string[] QueryValues)
    {
        Console.WriteLine(Query);
            
        for(int i = 0; i < QueryValues.Length; i++)
            Console.WriteLine(Convert.ToString(i+1) + ": " + QueryValues[i]);

        string inp = Console.ReadLine().Trim('"').Trim(' ');
        int[] inputs = Utils.getIntFromCommaLine(inp) ;

        for (int i = 0; i < inputs.Length; i++)
        {
            inputs[i]--;
            if (inputs[i] > QueryValues.Length+1 || inputs[i] < 0) 
            { 
                Console.WriteLine("This Option does not exist"); 
                return OptionSelectQueryMultiple(Query, QueryValues);
            } 
        }
        
        Console.Clear();

        return inputs;
    }
        
    public static int OptionSelectQuery(string Query, int ArrayLen)
    {
        Console.WriteLine(Query);
            
        int n;
            
        if (!int.TryParse(Console.ReadLine().Trim('"').Trim(' '), out n))
            return OptionSelectQuery(Query, ArrayLen);
            
        if (n >= ArrayLen || n < 0) 
        { 
            Console.WriteLine("This Option does not exist"); 
            return OptionSelectQuery(Query, ArrayLen);
        }            
            
        Console.Clear();

        return n;
    }   

    public static bool YesNoQuery(string Query)
    {
        Console.WriteLine(Query);
        string input = Console.ReadLine().ToLower().Trim();

        if (input == "y")
            return true;

        return false;
               
    }
    
    public static List<Extension> GetExtensionsWiConsole()
    {
        List<Extension> extList = new List<Extension>();

        for(int i=0 ;;i++)
        {
            if (i == 0)
            {
                extList.AddRange(Extension.GetExtByMode()); 
                continue;
            }
            
            Console.WriteLine("Extensions:");
            
            for (int j = 0; j < extList.Count; j++)
                Console.WriteLine(j + ": " + extList[j].ToString());

            int queryRes = ConsoleQueries.OptionSelectQuery("\nSelect the next option:",
                new[] { "add another ext", "remove ext", "edit ext","leave" });
            
            switch (queryRes)
            {
                case 1:
                    List<Extension> NewExts = Extension.GetExtByMode();
                    Console.Clear();
                    if (!Utils.ExtinExtension(NewExts, NewExts))
                        extList.AddRange(NewExts);
                    else
                        Console.WriteLine("Duplicate detected, please try again!");
                    break;
                case 2:
                    List<string> ExtStr = new List<string>();
                    extList.ForEach(x => ExtStr.Add(x.ToString()));
                    int ListSel = ConsoleQueries.OptionSelectQuery("Which element do you want to remove?", ExtStr.ToArray()) - 1;
                    extList.RemoveAt(ListSel);
                    break;
                case 3:
                    List<string> ExtStr2 = new List<string>();
                    extList.ForEach(x => ExtStr2.Add(x.ToString()));
                    int ListSel2 = ConsoleQueries.OptionSelectQuery("Which element do you want to edit?", ExtStr2.ToArray()) - 1;
                    extList[ListSel2] = Extension.ExtEditMode(extList[ListSel2],extList[ListSel2].Name);
                    break;
                case 4:
                    return extList;
            }
        }
    }
}