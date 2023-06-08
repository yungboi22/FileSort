namespace Configurator
{
    public static class ConsoleQueries
    {
        public static string StringQuery(string Query, bool clear, bool newLine)
        {
            Console.Write(Query);
            if(newLine)
                Console.Write("\n");
            
            string output = Console.ReadLine().Trim('"').Trim(' ');
            
            if(clear)
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

        public static string DirectoryPthQuery(string Query)
        {
            string output = StringQuery(Query,true,true);
            Console.WriteLine(output);            
            
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
                return DirectoryPthQuery(Query);
            }
        }
        
        public static string FilePthQuery(string Query)
        {
            string output = StringQuery(Query,true,true);

            if (File.Exists(output))
                return output;

            try
            {
                File.Create(output);
                Console.Clear();
                return output;
            }
            catch (IOException)
            {
                Console.WriteLine("File does not exist and could not be created!\n");
                return FilePthQuery(Query);
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
                Console.WriteLine("This Option does not exist");
                return OptionSelectQuery(Query, QueryValues);
            }

            Console.Clear();

            return n;

        }

        public static int[] OptionSelectQueryMultiple(string Query, string[] QueryValues)
        {
            Console.WriteLine(Query);

            for (int i = 0; i < QueryValues.Length; i++)
                Console.WriteLine(Convert.ToString(i + 1) + ": " + QueryValues[i]);

            string inp = Console.ReadLine().Trim('"').Trim(' ');
            int[] inputs = Utils.getIntFromCommaLine(inp);

            for (int i = 0; i < inputs.Length; i++)
            {
                inputs[i]--;
                if (inputs[i] > QueryValues.Length + 1 || inputs[i] < 0)
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
        
    }
}

