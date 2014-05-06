using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Wpf_WcfClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            log4net.Config.XmlConfigurator.Configure();
            log.Error("OnLoadCompleted setting up error handlers../.");

            // Add the event handler for handling UI thread exceptions to the event.
            // Application.ThreadException += new ThreadExceptionEventHandler(ExceptionDialog.UIThreadException);
            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            // Application.Current.
            //// Set the unhandled exception mode to force all Windows Forms errors to go through
            //// our handler.
            //Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            //// Add the event handler for handling non-UI thread exceptions to the event. 
            //AppDomain.CurrentDomain.UnhandledException +=
            //    new UnhandledExceptionEventHandler(MyApp.CurrentDomain_UnhandledException);
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // throw new NotImplementedException();
            log.Error("CurrentDomain_UnhandledException");
            
        }

        void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            // throw new NotImplementedException();
            log.Error("Current_DispatcherUnhandledException [" + e.Exception.Message + "] " + e.Exception.StackTrace);
            e.Handled = true;
        }
    }
    
}
