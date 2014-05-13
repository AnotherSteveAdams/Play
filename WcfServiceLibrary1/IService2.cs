using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfServiceLibrary1
{

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(ISampleClientCallbackContract2))]
    public interface IService2
    {
        [OperationContract(IsOneWay = true, IsInitiating = false, IsTerminating = false)]
        void PublishPriceChange2(string item, double price, double change);
    }

    public interface ISampleClientCallbackContract2
    {
        [OperationContract(IsOneWay = true)]
        void PriceChange2(IEnumerable<object> updatedObjects);
    }

    public class somethingToGo2
    {
        public string sss { get; set; }
        public override string ToString()
        {
            return sss;
        }
    }
}
