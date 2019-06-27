using System;
using System.Collections.Generic;
using System.Text;

namespace GBM.Portfolio.Model.Events
{
    public class EventFreeze : Event
    {
        public string InstrumentId { get; set; }
        public string InstrumentName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Side Side { get; set; }
    }

    public enum Side { Buy, Sell };
}
