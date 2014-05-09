﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WcfServiceLibrary1;

namespace WcfServiceLibrary1
{
    public enum SubscriptionId { First, Second } ;

    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface IWCFSubscribableService
    {
        // TODO: Add your service operations here
        [OperationContract(IsOneWay = false, IsInitiating = true)]
        object Subscribe(SubscriptionId id);

        [OperationContract(IsOneWay = false, IsTerminating = true)]
        void Unsubscribe();

    }
    [ServiceContract]
    public interface ISampleClientCallbackContract
    {
        [OperationContract]
        void PriceChange(IEnumerable<object> list);
    }

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
            binding.SendTimeout = new TimeSpan(0, 0, 0, 3);
            binding.ReceiveTimeout = new TimeSpan(0, 0, 0, 3); ;// TimeSpan.MaxValue;
            binding.OpenTimeout = new TimeSpan(0, 0, 0, 3);
            binding.CloseTimeout = new TimeSpan(0, 0, 0, 3); 
            _factory = new DuplexChannelFactory<TServerInterface>(
                    this,
                    binding,
                    "net.tcp://SteveX1:9966/TheWCFService/IService1"
                );
            _myservice = _factory.CreateChannel();
            var v = _factory.State;
            ((ICommunicationObject)_myservice).Faulted += MainWCFTestVM_Faulted;

            var v2 = _myservice.Subscribe(SubscriptionId.First);
            //StatusText = "Connected";
            ServiceUp = true;
        }

        // Add the unhandled exception handlers
        // remove ping in interface
        // make an base class to wrap up the reconnect logic.

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
