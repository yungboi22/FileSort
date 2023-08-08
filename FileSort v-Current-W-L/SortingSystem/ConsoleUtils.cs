using System.Text.Json;

namespace SortingSystem
{
    public static class ConsoleUtils
    {
        public static string StringQuery(string Query,bool outToNewLine)
        {
            if(outToNewLine)
                Console.Write(Query + "\n");
            else
                Console.Write(Query); 
            
            string output = Console.ReadLine().Trim('"').Trim(' ');
            
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

        public static string DirectoryPthQuery(string Query)
        {
            string output = StringQuery(Query,true);
            //Console.WriteLine(output);            
            
            if (Directory.Exists(output))
                return output;

            try
            {
                if (YesNoQuery("\nThis directory does not exist. Do you want to create " + output + "? (y/n)"))
                {
                    Directory.CreateDirectory(output);
                    return output;
                }
                
                return DirectoryPthQuery(Query);
            }
            catch (IOException)
            {
                Console.WriteLine("\nDirectory does not exist and could not be created!\n");
                return DirectoryPthQuery(Query);
            }
        }
        
        public static string FilePthQuery(string Query)
        {
            string output = StringQuery(Query,true);

            if (File.Exists(output))
                return output;

            try
            {
                File.Create(output);
                return output;
            }
            catch (IOException)
            {
                Console.WriteLine("File does not exist and could not be created!\n");
                return FilePthQuery(Query);
            }
        }
        
        public static string JsonFileQuery(string Query, bool isList)
        {
            string output = StringQuery(Query,true);

            if (output == "!q")
                return output;

            Extension extension = new Extension();
            
            try
            {
                if (!File.Exists(output))
                    throw new Exception("\nFile does not exist");
                if (Path.GetExtension(output) != ".json")
                    throw new Exception("\n" + Path.GetFileName(output) + " has a wrong file-format");

                if (!isList)
                {
                    Extension testExt = new Extension(output);
                }
                else
                {
                    Extension.LoadListFromJson(output);
                }

                return output;
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);
                return JsonFileQuery(Query,isList);
            }
        }
        

        public static int OptionSelectQuery(string Query, string[] QueryValues)
        {
            Console.WriteLine(Query);

            for (int i = 0; i < QueryValues.Length; i++)
                Console.WriteLine(Convert.ToString(i + 1) + ": " + QueryValues[i]);

            int n;

            if (!int.TryParse(Console.ReadLine().Trim('"').Trim(' '), out n))
                return OptionSelectQuery(Query, QueryValues);

            if (n >= QueryValues.Length + 1 || n <= 0)
            {
                Console.WriteLine("\nThis Option does not exist");
                return OptionSelectQuery(Query, QueryValues);
            }
            
            return n-1;
        }

        public static int[] OptionSelectQueryMultiple(string Query, string[] QueryValues)
        {
            Console.WriteLine(Query);

            for (int i = 0; i < QueryValues.Length; i++)
                Console.WriteLine(Convert.ToString(i + 1) + ": " + QueryValues[i]);

            string inp = Console.ReadLine().Trim('"').Trim(' ');
            List<int> inputs = new List<int>();

            for (int i = 0; i < inp.Split(",").Length ; i++)
            {
                int n;
                
                if (!int.TryParse(inp.Split(",")[i], out n))
                    return OptionSelectQueryMultiple(Query, QueryValues);
                
                if(n >= QueryValues.Length + 1 || n <= 0)
                {
                    Console.WriteLine("\nThis Option does not exist");
                    return OptionSelectQueryMultiple(Query, QueryValues);
                }
                
                inputs.Add(n-1);
            }
            
            return inputs.ToArray();
        }
        
        public static bool YesNoQuery(string Query)
        {
            Console.WriteLine(Query);
            string input = Console.ReadLine().ToLower().Trim();

            if (input == "y")
                return true;

            return false;
        }
    }

    public class ConsoleTable
    {
        private static int tableWidht;
        private static int columnCount;
        private List<List<string>> rows;


        public ConsoleTable(string[] head,int aSize)
        {
            tableWidht = aSize;
            columnCount = head.Length;
            rows = new List<List<string>>();
            rows.Add(head.ToList());
        }

        public void AddRow(List<string> row)
        {
            if (row.Count == columnCount)
                rows.Add(row);
            else
                throw new Exception("Row is out of range");
        }

        public void Print()
        {
            PrintLine();
            PrintRow(rows[0].ToArray());    //Head
            PrintLine();
            
            for (int i = 1; i < rows.Count; i++)
                PrintRow(rows[i].ToArray());
            
            PrintLine();
        }

        private void PrintLine()
        {
            Console.WriteLine(new string('-',tableWidht));
        }

        private void PrintRow(params string[] columns)
        {
            int width = (tableWidht - columns.Length) / columns.Length;
            string row = "|";
            
            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }
            
            Console.WriteLine(row);
        }

        private string AlignCentre(string text, int widht)
        {
            text = text.Length > widht ? text.Substring(0, widht - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', widht);
            }
            else
            {
                return text.PadRight(widht - (widht - text.Length) / 2).PadLeft(widht);
            }
        }
    }
}

