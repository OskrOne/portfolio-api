using System;
using Newtonsoft.Json.Linq;

namespace GBM.Portfolio.Domain.Models.Events
{
    internal class EventJsonConverter : JsonCreationConverter<Event>
    {
        protected override Event Create(Type objectType, JObject jObject)
        {
            if (jObject == null) throw new ArgumentNullException("jObject is null");

            if (jObject["EventType"] != null && jObject["EventType"].Value<string>() == EventType.AddMoney.ToString())
            {
                return new AddMoney();
            }
            else if (jObject["EventType"] != null && jObject["EventType"].Value<string>() == EventType.Freeze.ToString())
            {
                return new Freeze();
            }
            else {
                return new Event();
            }
        }
    }
}