using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfServiceLibrary1
{

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(ISampleClientCallbackContract))]
    public interface IService1 : IWCFSubscribableService
    {
        [OperationContract(IsInitiating=false, IsTerminating=false)]
        string GetData(int value);

        [OperationContract(IsInitiating = false, IsTerminating = false)]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        [OperationContract(IsOneWay = true, IsInitiating = false, IsTerminating = false)]
        void PublishPriceChange(string item, double price, double change);


    }


    public interface ISampleClientCallbackContract
    {
        [OperationContract(IsOneWay = true)]
        void PriceChange(string item, double price, double change);
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    // You can add XSD files into the project. After building the project, you can directly use the data types defined there, with the namespace "WcfServiceLibrary1.ContractType".
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
