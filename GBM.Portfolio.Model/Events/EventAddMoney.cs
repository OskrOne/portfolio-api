using System;
using System.Collections.Generic;
using System.Text;

namespace GBM.Portfolio.Model.Events
{
    public class EventAddMoney : Event
    {
        public decimal Money { get; set; }

        public EventAddMoney() {
        }
    }
}
