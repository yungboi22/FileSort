using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Diagnostics;
using System.Threading;

namespace Configurator;

public static class FileSort
{
    public static void Start()
    {
        try
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                ServiceController sc = new ServiceController();
                sc.ServiceName = "SortingSystem";

                if (sc.Status == ServiceControllerStatus.Stopped)
                    sc.Start();
            }
            else
                Utils.excuteScript("Scripts/start.sh");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine("Try to run as administrator");
            Console.ReadKey();
            throw;
        }
        
    }
    
    public static void Stop()
    {
        try
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                ServiceController sc = new ServiceController();
                sc.ServiceName = "SortingSystem";

                if (sc.Status == ServiceControllerStatus.Running)
                    sc.Stop();
            }
            else
                Utils.excuteScript("Scripts/stop.sh");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine("Try to run as administrator");
            Console.ReadKey();
            throw;
        }
        
        
    }
    
    public static void Restart()
    {
        try
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                ServiceController sc = new ServiceController();
                sc.ServiceName = "SortingSystem";

                if (sc.Status == ServiceControllerStatus.Running)
                    sc.Stop();
            }
            else
                Utils.excuteScript("Scripts/restart.sh");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine("Try to run as administrator");
            throw;
        }
       
    }
}
