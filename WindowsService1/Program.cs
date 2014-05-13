using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WindowsService1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                var ds = new Service1();
                ds.StartInDebugMode();
                while (true)
                {
                    System.Threading.Thread.Sleep(1000);
                }
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
            { 
                new Service1() 
            };
                ServiceBase.Run(ServicesToRun);
            }
        }


    }
}
