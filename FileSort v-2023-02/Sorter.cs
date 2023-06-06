using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Timers;
using Timer = System.Timers.Timer;


namespace Sorting_Service_13_02_2023_1_0
{
    public class Sorter
    {
        public static List<Sorter> Sorters { get; set; }
        public string Name { get; set; }
        public string ScanDir { get; set; }
        public string EndDir { get; set; }
        public List<Extension> Extensions { get; set; }
        public string[] StaticDirs { get; set; }
        private Timer timer { get; set; }
        public bool DeleteFromDesktop { get; set; }
        public bool SortByYearMonth { get; set; }
      

        public Sorter(string aName, string aScanDir,string aEndDir, List<Extension> aExtensions, string[] aStaticDirs,bool aDeleteFromDesktop, bool asortByYearMonth)
        {
            aExtensions = Extension.AddDefaultValues(aExtensions);
            
            Name = aName;
            ScanDir = aScanDir;
            EndDir = aEndDir;
            Extensions = aExtensions;
            StaticDirs = aStaticDirs;
            DeleteFromDesktop = aDeleteFromDesktop;
            SortByYearMonth = asortByYearMonth;
            
            timer = new Timer(1000);
            //timer.Elapsed += OnTimedEvent;
        }

        public Sorter()
        {
            timer = new Timer(1000);
            //timer.Elapsed += OnTimedEvent;
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
        
        //public void OnTimedEvent(object source, ElapsedEventArgs e)
        
        /// <summary>
        /// Timer-Event on tick
        /// </summary>
        public void OnTimed()
        {
            try
            {
                if (IOMethod.NewItemLoc(ScanDir,StaticDirs))
                {
                    string[] Items = IOMethod.GetNewItems(ScanDir,StaticDirs);
                
                    if(DeleteFromDesktop)
                        IOMethod.RemoveFromDesktop(Items);
                
                    IOMethod.CreateEndFolders(Extensions,EndDir);

                    for (int i = 0; i < Items.Length; i++) 
                    {
                        IOMethod.MoveItem(Items[i], EndDir,SortByYearMonth,this);
                    }
                }
            }
            catch (Exception exception)
            {
                Logs.add(exception.Message,Name);
                timer.Stop();
                throw;
            }
        }
        
        

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
        
        public static void stopSorters(List<Sorter> sorters) 
        {
            foreach (Sorter sorter in sorters) 
            {
                sorter.timer.Stop();
            }
        }
        public static void startSorters(List<Sorter> sorters) 
        {
            foreach (Sorter sorter in sorters) 
            {
                sorter.timer.Start();
            }
        }
    }
}

