using System;
using Newtonsoft.Json;

namespace GBM.Portfolio.Model.Events
{
    [JsonConverter(typeof(EventJsonConverter))]
    public class Event
    {
        public EventType EventType { get; set; }
        public string ContractId { get; set; }
        public string TimeSpan { get; set; }

        public Event() {
        }
    }

    public enum EventType { Freeze, AddMoney };
}
