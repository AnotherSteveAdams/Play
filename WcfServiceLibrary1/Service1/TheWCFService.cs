using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfServiceLibrary1
{
   
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    [ServiceBehavior(IncludeExceptionDetailInFaults=true, ConcurrencyMode=ConcurrencyMode.Reentrant)]
    public class TheWCFService : IService1
    {
        //private static readonly ILog Logger = LogManager.GetLogger(typeof(TheWCFService));
        public static event PriceChangeEventHandler PriceChangeEvent;
        public delegate void PriceChangeEventHandler(object sender, PriceChangeEventArgs e);

        ISampleClientCallbackContract1 callback = null;

        //Clients call this service operation to subscribe.
        //A price change event handler is registered for this client instance.

        public void Subscribe()
        {
            callback = OperationContext.Current.GetCallbackChannel<ISampleClientCallbackContract1>();
            PriceChangeEvent += PriceChangeHandler;
            //return "abc";
            //return new List<somethingToGo> {
              //  new somethingToGo{sss = "to be returned" }}.AsEnumerable<object>();
            //return new List<object>() { new  somethingToGo{sss = "to be returned" } }.AsEnumerable<object>();
            //var d = new Dictionary<Tuple<int,int>, 
            //return ;//new List<somethingToGo1>() { new  somethingToGo1{sss = "to be returned" } };
            //return new Dictionary<int, string> { { 1, "abc" } };
        }

        //Clients call this service operation to unsubscribe.
        //The previous price change event handler is deregistered.

        public void Unsubscribe()
        {
            PriceChangeEvent -= PriceChangeHandler;
        }

        //Information source clients call this service operation to report a price change.
        //A price change event is raised. The price change event handlers for each subscriber will execute.

        public void PublishPriceChange(string item, double price, double change)
        {
            PriceChangeEventArgs e = new PriceChangeEventArgs();
            e.Item = item;
            e.Price = price;
            e.Change = change;
            if (PriceChangeEvent != null)
                PriceChangeEvent(this, e);
        }

        //This event handler runs when a PriceChange event is raised.
        //The client's PriceChange service operation is invoked to provide notification about the price change.

        public void PriceChangeHandler(object sender, PriceChangeEventArgs e)
        {
            try
            {
                IEnumerable<object> o = new List<string> { "addd" };
                callback.PriceChange(new List<somethingToGo1> { new somethingToGo1 { sss = "strawberry" } });
            }
            catch (CommunicationObjectAbortedException ex)
            {
                //Logger.Error("Exception in PriceChangeHandler trying to fire a change[" + ex.Message + "]");
                PriceChangeEvent -= PriceChangeHandler;
            }
        }



    }
}
