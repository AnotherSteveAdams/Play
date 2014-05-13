using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WcfServiceLibrary1;

namespace WcfServiceLibrary1
{
    //public enum SubscriptionId { First, Second } ;

    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface IWCFSubscribableService
    {
        // TODO: Add your service operations here
        [OperationContract(IsOneWay = false, IsInitiating = true)]
        void Subscribe();

        [OperationContract(IsOneWay = false, IsTerminating = true)]
        void Unsubscribe();
    }
    //// This class has the method named GetKnownTypes that returns a generic IEnumerable. 
    //static class Helper
    //{
    //    public static IEnumerable<Type> GetKnownTypes(ICustomAttributeProvider provider)
    //    {
    //        System.Collections.Generic.List<System.Type> knownTypes =
    //            new System.Collections.Generic.List<System.Type>();
    //        // Add any types to include here.
    //        knownTypes.Add(typeof(somethingToGo));
    //        return knownTypes;
    //    }
    //}


    public class WCFReconnectingCallbackClient<TServerInterface, TCallbackInterface> : INotifyPropertyChanged
        where TServerInterface : class, IWCFSubscribableService
    {
        public WCFReconnectingCallbackClient()
        {
            Task.Run(() => CreateChannel());
        }

        ~WCFReconnectingCallbackClient()
        {
            _myservice.Unsubscribe();
        }

        private bool _serviceUp = false;
        public bool ServiceUp
        {
            get { return _serviceUp;}
            set { _serviceUp = value; OnPropertyChanged("ServiceUp"); }
        }
          
        protected void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected TServerInterface _myservice;
        void CreateChannel()
        {
            var binding = new NetTcpBinding(SecurityMode.None);
            binding.SendTimeout = new TimeSpan(0, 0, 0, 13);
            binding.ReceiveTimeout = new TimeSpan(0, 0, 0, 13); ;// TimeSpan.MaxValue;
            binding.OpenTimeout = new TimeSpan(0, 0, 0, 13);
            binding.CloseTimeout = new TimeSpan(0, 0, 0, 13); 
            _factory = new DuplexChannelFactory<TServerInterface>(
                    this,
                    binding,
                    // "net.tcp://SteveX1:9966/TheWCFService/IService1"
                     "net.tcp://citlonwksca91.pceservices.net:9966/TheWCFService/IService1"
                );
            _myservice = _factory.CreateChannel();
            var v = _factory.State;
            ((ICommunicationObject)_myservice).Faulted += MainWCFTestVM_Faulted;

            // var v1 = xxx(SubscriptionId.Second);
            _myservice.Subscribe();
            //StatusText = "Connected";
            //v2.ToList().ForEach(p => Console.WriteLine(p));
            ServiceUp = true;
        }

        DuplexChannelFactory<TServerInterface> _factory;
        void MainWCFTestVM_Faulted(object sender, EventArgs e)
        {
//todo             StatusText = "Faulted";
            ServiceUp = false;
            ((ICommunicationObject)sender).Abort();
            if (sender is TServerInterface)
           {
                System.Threading.Thread.Sleep(2000);
                Task.Run(() => CreateChannel());
            }
        }
    }
}
