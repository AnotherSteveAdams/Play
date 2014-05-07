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
    public class TheWCFService : IService1
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public static event PriceChangeEventHandler PriceChangeEvent;
        public delegate void PriceChangeEventHandler(object sender, PriceChangeEventArgs e);

        ISampleClientCallbackContract callback = null;

        //Clients call this service operation to subscribe.
        //A price change event handler is registered for this client instance.

        public void Subscribe()
        {
            callback = OperationContext.Current.GetCallbackChannel<ISampleClientCallbackContract>();
            PriceChangeEvent += PriceChangeHandler;
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
                callback.PriceChange(e.Item, e.Price, e.Change);
            }
            catch (CommunicationObjectAbortedException ex)
            {
                PriceChangeEvent -= PriceChangeHandler;
            }
        }



    }
}
