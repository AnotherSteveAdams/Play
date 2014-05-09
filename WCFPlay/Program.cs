using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WcfServiceLibrary1;

namespace WCFPlay
{
    class Program
    {
        static void Main(string[] args)
        {
            //var s = new ServiceReference1.Service1Client();

            //var x = s.GetData(12);

            var binding = new NetTcpBinding(SecurityMode.None);
            binding.SendTimeout = new TimeSpan(0, 0, 0, 30);
            binding.ReceiveTimeout = TimeSpan.MaxValue;
            var factory = new DuplexChannelFactory<IService1>(
                    new CallbackClass(),
                    binding,
                    "net.tcp://SteveX1:9966/TheWCFService/IService1"
                );
            IService1 c = factory.CreateChannel();
            c.Subscribe(SubscriptionId.First);
            c.PublishPriceChange("ABC",123,32);
            System.Threading.Thread.Sleep(1000000);
        }
    }
   
    class CallbackClass : ISampleClientCallbackContract
    {

        [OperationContract(IsOneWay = true)]
        public void PriceChange(IEnumerable<object> updatedObjects)
        {
            //throw new NotImplementedException();
        }
    }
}
