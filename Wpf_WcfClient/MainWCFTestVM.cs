using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WcfServiceLibrary1;

namespace Wpf_WcfClient
{
    class x : IObservable<string>
    {

        public IDisposable Subscribe(IObserver<string> observer)
        {
            throw new NotImplementedException();
        }
    }

    class MainWCFTestVM : ISampleClientContract, INotifyPropertyChanged
    {
        public MainWCFTestVM()
        {

            CreateChannel();

        }
        void CreateChannel()
        {
            var binding = new NetTcpBinding(SecurityMode.None);
            binding.SendTimeout = new TimeSpan(0, 0, 0, 3);
            binding.ReceiveTimeout = new TimeSpan(0, 0, 0, 3); ;// TimeSpan.MaxValue;
            binding.OpenTimeout = new TimeSpan(0, 0, 0, 3);
            binding.CloseTimeout = new TimeSpan(0, 0, 0, 3); 
            _factory = new DuplexChannelFactory<IService1>(
                    this,
                    binding,
                    "net.tcp://SteveX1:9966/TheWCFService/IService1"
                );
            _myservice = _factory.CreateChannel();

            ((ICommunicationObject)_myservice).Faulted += MainWCFTestVM_Faulted;

            try
            {
                _myservice.Subscribe();
            }
            catch (EndpointNotFoundException ex)
            {

            }
            StatusText = "Connected";
            ServiceUp = true;
            OnPropertyChanged("ServiceUp");
        }
        DuplexChannelFactory<IService1> _factory;
        void MainWCFTestVM_Faulted(object sender, EventArgs e)
        {
            StatusText = "Faulted";
            ServiceUp = false;
            OnPropertyChanged("ServiceUp");
            ((ICommunicationObject)sender).Abort();
            if (sender is IService1)
            {
                System.Threading.Thread.Sleep(500);
                System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() => CreateChannel()), null);
                
            }
        }
        public string StatusText
        {
            get { return _statusText; }
            set { _statusText = value; OnPropertyChanged("StatusText"); }
        }
        ~MainWCFTestVM()
        {
            _myservice.Unsubscribe();
        }

        public string Message
        {
            get;
            set;
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set 
            { 
                _errorMessage = value; 
                if (_errorMessage != "")
                {
                    InError = true;
                    OnPropertyChanged("InError");
                }
                OnPropertyChanged("ErrorMessage"); 
            }
        }

        public bool ServiceUp
        {
            get;
            set;
        }
        public bool InError
        {
            get;
            set;
        }

        IService1 _myservice;
        ICommand _myCommand;
        public ICommand CommandSendPrice
        {
            get
            {
                if (_myCommand == null)
                {
                    _myCommand = new RelayCommand(p =>
                    {
                        if (Message.Contains("Jacob"))
                        {
                            ErrorMessage = "You are a burglar, system will self destruct in 5 seconds";

                            Task.Run(() =>
                                {
                                    for (int timeToGo = 5; timeToGo >= 2; timeToGo -= 1)
                                    {
                                        System.Threading.Thread.Sleep(1000);
                                        ErrorMessage = "You are a burglar, system will self destruct in " + timeToGo + " seconds";
                                    }

                                    System.Threading.Thread.Sleep(1000);
                                    ErrorMessage = "Self destruct deactivated, sorry I thougth you were trying to steal me !";

                                });
                            return;
                        }
                        if (Message.Contains("Bloody"))
                        {
                            ErrorMessage = "Don't say the word Bloody, okay !  Message was not sent";
                            return;
                        }
                        _myservice.PublishPriceChange(Message, 123, 32);
                    });
                }
                return _myCommand;
            }
        }

        void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<string> _theList = new ObservableCollection<string>();
        public ObservableCollection<string> TheList
        {
            get { return _theList;  }
        }
        public void PriceChange(string item, double price, double change)
        {
            _theList.Add(item + " " + price + " " + change);
        }

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private string _statusText;
        private void test()
        {
            
        }
    }
}
