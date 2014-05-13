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

    // https://github.com/AnotherSteveAdams/Play.git
    class UserControl1VM : WCFReconnectingCallbackClient<IService2, ISampleClientCallbackContract2>, ISampleClientCallbackContract2
    {
        public UserControl1VM()
            : base("The2ndWCFService")
        {
            PropertyChanged += UserControl1VM_PropertyChanged;
        }

        void UserControl1VM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ServiceUp")
                OnPropertyChanged("StatusText");
        }
        public string StatusText
        {
            get { return _statusText; }
            set { _statusText = value; OnPropertyChanged("StatusText"); }
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
                    //InError = true;
                    OnPropertyChanged("InError");
                }
                OnPropertyChanged("ErrorMessage"); 
            }
        }

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
                        _myservice.PublishPriceChange2(Message, 123, 32);
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

        public void PriceChange2(List<somethingToGo2> list)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() => list.ForEach(l => _theList.Add(l.sss))), null);
                //   list.ForEach(l => _theList.Add(l.sss))
            //System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() => _theList.Add(item + " " + price + " " + change)), null);
            //;
        }

        private string _statusText;
        private void test()
        {
            
        }
    }
}
