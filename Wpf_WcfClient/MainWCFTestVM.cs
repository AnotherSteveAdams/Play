using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Windows.Input;
using WcfServiceLibrary1;

namespace Wpf_WcfClient
{
    class MainWCFTestVM : ISampleClientContract
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
                        c.PublishPriceChange("ABC", 123, 32);
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
    }
}
