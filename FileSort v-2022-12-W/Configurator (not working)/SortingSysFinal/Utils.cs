namespace SortingSysFinal;

public static class Utils
{
    public static int[] getIntFromCommaLine(string inp)
    {
        string[] inputs = inp.Split(",");
        int[] outputs = new int[inputs.Length];

        for (int i = 0; i < inputs.Length; i++)
        {
            int n;

            if (!int.TryParse(inputs[i], out n))
                return getIntFromCommaLine(inp);

            outputs[i] = n;
        }

        return outputs;
    }

    public static List<string> getItems(string DirPath)
    {
        List<string> items = new List<string>();
        items.AddRange(Directory.GetDirectories(DirPath));
        items.AddRange(Directory.GetFiles(DirPath));

        return items;
    }

    public static List<string> selectItems(List<string> items, int[] values)
    {
        List<string> outlist = new List<string>();

        foreach (int value in values)
        {
            outlist.Add(items[value]);
        }

        return outlist;
    }
    
    public static bool ExtinExtension(List<Extension> extensions, List<Extension> CompExts)
    {
        foreach (Extension ext in extensions)
        {
            foreach (Extension compExt in CompExts)
            {
                if (ext.Name == compExt.Name)
                {
                    return true;  
                }
                   
            }
        }

        return false;
    }
}