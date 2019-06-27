using System;
using GBM.Portfolio.API.EventSourcing.Handlers;
using GBM.Portfolio.DataProvider;
using GBM.Portfolio.Model.Events;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using EventHandler = GBM.Portfolio.API.EventSourcing.Handlers.EventHandler;

namespace GBM.Portfolio.API.Controllers
{
    [Route("api/portfolio/")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private ProviderConfig ProviderConfig;

        public PortfolioController(IConfiguration configuration)
        {
            ProviderConfig = new ProviderConfig()
            {
                Local = bool.Parse(configuration["AWS:Local"]),
                DynamoDBURL = configuration["AWS:DynamoDBURL"],
                AwsAccessKeyId = configuration["AWS:AccessKeyId"],
                AwsSecretAccessKey = configuration["AWS:SecretAccessKey"],
                RegionEndpoint = Amazon.RegionEndpoint.USWest2 // TODO: Include this configuration in AppSettings
            };
        }


        [HttpPut("event")]
        public void Event([FromBody] Event @event)
        {
            var handler = GetHandler(@event);
            @event.TimeSpan = GetTimestamp(DateTime.Now);
            handler.Handle(@event);
        }

        private EventHandler GetHandler(Event @event)
        {
            if (@event.GetType() == typeof(EventFreeze)) {
                return new FreezeEvent(ProviderConfig);
            }

            if (@event.GetType() == typeof(EventAddMoney)) {
                return new AddMoneyEvent(ProviderConfig);
            }
            return null; // TODO: Handle exceptions
        }

        private string GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }
    }
}