﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text.RegularExpressions;

namespace SortingSystem
{
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

        public static void excuteScript(string pth)
        {
            Process script = new Process();
            script.StartInfo.FileName = pth;
            script.StartInfo.Arguments = "sudo";
            script.StartInfo.WorkingDirectory = "Scripts/";
            script.Start();
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
        
        public static string CheckName(string ItemName, List<string> CompItems)
        {
            foreach (string CompItem in CompItems)
            {
                if (CompItem == ItemName)
                {
                    if (BracketNumberFormat(ItemName))
                        ItemName = ReplaceNum(ItemName, GetFileNameNum(ItemName)+1);
                    else
                        ItemName+= " (" + 2 + ")";
                   
                    ItemName = CheckName(ItemName, CompItems);
                }
            }
            
            return ItemName;
        }

        private static int GetFileNameNum(string input)
        {
            Regex rgx = new Regex("[^0-9 -]");
            input = rgx.Replace(input, "");
            
            return Convert.ToInt32(input);
        }
        
        private static bool BracketNumberFormat(string input)
        {
            var output = Regex.Replace(input, @"[\d-]", string.Empty);
            if (output.Contains("()") && input.Any(char.IsDigit))
                return true;
                
            return false;
        }
        
        private static string ReplaceNum(string input,int NewNum)
        {
            string output = Regex.Replace(input, @"[\d-]", string.Empty);
            output = output.Substring(0, output.IndexOf("()"));
            output += "(" + NewNum+ ")" ;

            return output;
        }
        
        public static bool IsAdministrator()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        public static int compSymbolToInt(char comparisonSymbol)
        {
            switch (comparisonSymbol)
            {
                case '>':
                    return 1;
                case '<':
                    return -1;
                case '=':
                    return 0;
                default:
                    return -99;
            }
        }
        
        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input[0].ToString().ToUpper() + input.Substring(1);
            }
        }

        public static void OpenPathInExplorer(string Pth)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                Arguments = @"E:\All",
                FileName = "explorer.exe"
            };

            Process.Start(startInfo);
        }
        
    }
    
}

