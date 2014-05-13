﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfServiceLibrary1
{

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(ISampleClientCallbackContract1))]
    public interface IService1 
    {
        [OperationContract(IsOneWay = true, IsInitiating = false, IsTerminating = false)]
        void PublishPriceChange(string item, double price, double change);

    }
    [ServiceContract]
    public interface ISampleClientCallbackContract1
    {
        [OperationContract]
        void PriceChange(List<somethingToGo1> list);
    }
    public class somethingToGo1
    {
        public string sss { get; set; }
        public override string ToString()
        {
            return sss;
        }
    }

}
