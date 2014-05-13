using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfServiceLibrary1
{
    public class PriceChangeEventArgs : EventArgs
    {
        public string Item;
        public double Price;
        public double Change;
    }


    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class The2ndWCFService : IService2
    {
        public static event PriceChangeEventHandler PriceChangeEvent;
        public delegate void PriceChangeEventHandler(object sender, PriceChangeEventArgs e);

        ISampleClientCallbackContract2 callback = null;

        //Clients call this service operation to subscribe.
        //A price change event handler is registered for this client instance.

        public void Subscribe()
        {
            callback = OperationContext.Current.GetCallbackChannel<ISampleClientCallbackContract2>();
            PriceChangeEvent += PriceChangeHandler;
            //return "abc";
            //return new List<somethingToGo> {
              //  new somethingToGo{sss = "to be returned" }}.AsEnumerable<object>();
            //return new List<object>() { new  somethingToGo{sss = "to be returned" } }.AsEnumerable<object>();
            //var d = new Dictionary<Tuple<int,int>, 
            //return;// new List<somethingToGo2> { new  somethingToGo2{sss = "to be returned" } };
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

        public void PublishPriceChange2(string item, double price, double change)
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
                callback.PriceChange2(o);
            }
            catch (CommunicationObjectAbortedException ex)
            {
                PriceChangeEvent -= PriceChangeHandler;
            }
        }



    }
}
