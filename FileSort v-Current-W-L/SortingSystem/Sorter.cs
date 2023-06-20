using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Timers;
using Timer = System.Timers.Timer;


namespace SortingSystem
{
    public class Sorter
    {
        public static List<Sorter> Sorters { get; set; }
        /*
         Solo static attribute, load all Sort-Systems from json to this on start.
         (Used by multiple other classes) 
        */

        public string Name { get; set; }
        public string ScanDir { get; set; }
        public string EndDir { get; set; }
        public List<Extension> Extensions { get; set; }
        public string[] StaticDirs { get; set; }
        public bool DeleteFromDesktop { get; set; }
        public bool SortByYearMonth { get; set; }
        public bool LinuxOS { get; set; }
        private bool Enabled { get; set; }
 
        
        
        public Sorter(string aName, string aScanDir, string aEndDir, List<Extension> aExtensions, string[] aStaticDirs, bool aDeleteFromDesktop, bool asortByYearMonth)
        {
            aExtensions = Extension.AddDefaultValues(aExtensions);
            
            Name = aName;
            ScanDir = aScanDir;
            EndDir = aEndDir;
            Extensions = aExtensions;
            StaticDirs = aStaticDirs;
            DeleteFromDesktop = aDeleteFromDesktop;
            SortByYearMonth = asortByYearMonth;
            Enabled = false;

            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                LinuxOS = false;
            else
                LinuxOS = true;
        }

        public Sorter()
        {

        }

        /// <summary>
        /// Loads a Sorter from a json-file using JsonSerializerClass
        /// </summary>
        /// <param name="Path">Filepath from json-file</param>
        /// <returns>Sorter</returns>
        public Sorter loadSorter(string Path)
        {
            string obj = File.ReadAllText(Path);
            var options = new JsonSerializerOptions { WriteIndented = true };
            Sorter sorter = JsonSerializer.Deserialize<Sorter>(obj,options)!;
            return sorter;
        }

        /// <summary>
        /// Saves a Sorter as a json-file using JsonSerializerClass
        /// </summary>
        /// <param name="Path">Filepath from json-file</param>
        public void saveSorter(string Path)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string obj = JsonSerializer.Serialize(this,options);
            File.WriteAllText(Path + ".json",obj);
        }


        /// <summary>
        /// Called by Worker or Timer
        /// </summary>
        public void OnTimed()
        {
            if (IOMethod.NewItemLoc(ScanDir, StaticDirs))
            {
                string[] Items = IOMethod.GetNewItems(ScanDir, StaticDirs, EndDir);

                if (DeleteFromDesktop)
                    IOMethod.RemoveFromDesktop(Items);

                IOMethod.CreateEndFolders(Extensions, EndDir);

                for (int i = 0; i < Items.Length; i++)
                {
                    IOMethod.MoveItem(Items[i], EndDir, SortByYearMonth, this);
                }
            }
        }
    
        
        /*                                          Static functions                                                  */

        public static List<Sorter> loadSorters(string Pth)
        {
            string obj = File.ReadAllText(Pth);
            var options = new JsonSerializerOptions { WriteIndented = true }; 
            return JsonSerializer.Deserialize<List<Sorter>>(obj,options)!;
        }
        
        public static void saveSorters(List<Sorter> aSorters,string Pth)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string obj = JsonSerializer.Serialize(aSorters,options);
            File.WriteAllText(Pth,obj);
        }
        
        public static void unableSorters(List<Sorter> sorters) 
        {
            foreach (Sorter sorter in sorters)
            {
                sorter.Enabled = false;
            }
        }
        
        public static void enableSorters(List<Sorter> sorters) 
        {
            foreach (Sorter sorter in sorters)
            {
                sorter.Enabled = true;
            }
        }
        
        public static void TickAll(List<Sorter> sorters)
        {
            foreach (Sorter sorter in sorters)
            {
                if(sorter.Enabled)
                    sorter.OnTimed();
            }
        }
        
    }
}

