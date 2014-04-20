using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
//using log4net;
using System.ServiceModel.Channels;
using WcfServiceLibrary1;
using log4net;

namespace WindowsService1
{
    public partial class Service1 : ServiceBase
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Service1));


        private ServiceHost _serviceHost;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // System.Threading.Thread.Sleep(60000);
            log4net.Config.XmlConfigurator.Configure();
            Logger.Info("I am starting");
            // Start up the service.
            _serviceHost = getServiceHost(typeof(IService1), typeof(TheWCFService), 9966);
            Logger.Info("Opening serviceshost");
            try
            {
                _serviceHost.Open();
            }
            catch (Exception e)
            {
                Logger.Error("Exception:" + e.Message);
            }
            Logger.Info("opened serviceshost");
        }

        protected override void OnStop()
        {
        }

        public static string formatTCPBaseAddress(string serviceName, int port)
        {
            return string.Format("net.tcp://{0}:{1}/{2}", GetHostNameIncludingDomain(), port, serviceName);
        }

        private static string GetHostNameIncludingDomain()
        {
            // Thanks to
            // http://stackoverflow.com/questions/804700/how-to-find-fqdn-of-local-machine-in-c-net
            string domainName = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
            string hostName = System.Net.Dns.GetHostName();
            string fqdn = "";
            if (!hostName.Contains(domainName))
                fqdn = hostName + "." + domainName;
            else
                fqdn = hostName;
            return fqdn;
        }

        public ServiceHost getServiceHost(Type serviceInterface, Type serviceClass, int port)//, IServiceLocator locator)
        {
            try
            {
                // net.tcp://SteveX1:9966/Service1/IService1

                //// Logger.Info("Creating service host:  Interface: [" + serviceInterface + "] Service Class: [" +
                  //          serviceClass + "] Port: [" + port + "] Locator: [" + locator + "]");
                Logger.Info("getServiceHost: 1");

                string baseAddress = formatTCPBaseAddress(serviceClass.Name, port);
                string fullAddress = baseAddress + "/" + serviceInterface.Name;
                EventLog.WriteEntry("Service", "Listening on [" + fullAddress + "]", EventLogEntryType.Information, 234);

                // // Logger.Info("Creating service host");
                //////////////////////////////////////////////
                ServiceHost serviceHost = new ServiceHost(serviceClass, new Uri(baseAddress));
                //TimeSpan o = serviceHost.OpenTimeout;
                //TimeSpan c = serviceHost.CloseTimeout;
                Logger.Info("getServiceHost: address [" + fullAddress + "]");

                // Logger.Info("End point is at [" + fullAddress + "]");
                NetTcpBinding b = new NetTcpBinding(SecurityMode.None);

                TimeSpan a1 = b.CloseTimeout;
                TimeSpan a2 = b.OpenTimeout;
                TimeSpan a3 = b.ReceiveTimeout;
                TimeSpan a4 = b.SendTimeout;

                b.ReceiveTimeout = TimeSpan.MaxValue;

                //////////////////////////////////////////////
                CustomBinding newBinding = new CustomBinding(b);
                TcpTransportBindingElement tcpBE = newBinding.Elements.Find<TcpTransportBindingElement>();
                // It looks from documentation like you can set these timeouts from the client side, but 
                // it doesn't work - had to set these on the orderouting server.  Leaving this code here
                // for future reference.
                // Logger.Info("Setting LeaseTimeout to max value");
                //tcpBE.ConnectionPoolSettings.LeaseTimeout =  new TimeSpan(0,0,2,0);
                tcpBE.ConnectionPoolSettings.LeaseTimeout = TimeSpan.MaxValue;
                // Logger.Info("Setting IdleTimeout to max value");
                //tcpBE.ConnectionPoolSettings.IdleTimeout = new TimeSpan(0, 0, 2, 0);
                tcpBE.ConnectionPoolSettings.IdleTimeout = TimeSpan.MaxValue;


                serviceHost.AddServiceEndpoint(serviceInterface, b, serviceInterface.Name);

                // locator.extendServiceHost(serviceHost);
                return serviceHost;
            }
            catch (Exception ex)
            {
                string errorMsg = "Track E: An application error occurred (not in windows thread). Please contact support " +
                    "with the following information:\n\n";
                // Logger.Error(errorMsg + ex.GetExceptionMessage());

                // Since we can't prevent the app from terminating, log this to the event log.
                if (!EventLog.SourceExists("OrderRouter Exception"))
                    EventLog.CreateEventSource("OrderRouter Exception", "Application");

                // Create an EventLog instance and assign its source.
                EventLog myLog = new EventLog();
                myLog.Source = "OrderRouter Exception";
                myLog.WriteEntry(errorMsg + ex.Message + "\n\nStack Trace:\n" + ex.StackTrace);

            }
            return null;
        }


    }
}
