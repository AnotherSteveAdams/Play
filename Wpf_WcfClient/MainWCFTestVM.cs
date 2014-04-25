using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
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

            var binding = new NetTcpBinding(SecurityMode.None);
            binding.SendTimeout = new TimeSpan(0, 0, 0, 30);
            binding.ReceiveTimeout = TimeSpan.MaxValue;
            var factory = new DuplexChannelFactory<IService1>(
                    this,
                    binding,
                    "net.tcp://SteveX1:9966/TheWCFService/IService1"
                );
            c = factory.CreateChannel();
            c.Subscribe();
            

        }

        ~MainWCFTestVM()
        {
            c.Unsubscribe();
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

        public bool InError
        {
            get;
            set;
        }

        IService1 c;
        ICommand _myCommand;
        public ICommand CommandSendPrice
        {
            get
            {
                if (_myCommand == null)
                {
                    _myCommand = new RelayCommand(p =>
                    {
                        if (Message.Contains("Bloody"))
                        {
                            ErrorMessage = "Don't say the word Bloody, okay !  Message was not sent";
                            return;
                        }
                        c.PublishPriceChange(Message, 123, 32);
                    });
                }
                return _myCommand;
            }
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
        private void test()
        {
            
        }
    }
}
