using System;
using GBM.Portfolio.Domain.Models.Events;
using GBM.Portfolio.Domain.Repositories;
using GBM.Portfolio.Domain.Repositories.Events;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GBM.Portfolio.API.Controllers
{
    [Route("api/portfolio/")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private RepositoryConfig ProviderConfig;

        public PortfolioController(IConfiguration configuration)
        {
            ProviderConfig = new RepositoryConfig()
            {
                Local = bool.Parse(configuration["AWS:Local"]),
                DynamoDBURL = configuration["AWS:DynamoDBURL"],
                AwsAccessKeyId = configuration["AWS:AccessKeyId"],
                AwsSecretAccessKey = configuration["AWS:SecretAccessKey"],
                RegionEndpoint = Amazon.RegionEndpoint.USWest2 // TODO: Include this configuration in AppSettings
            };
        }

        [HttpPut("event")]
        public ActionResult Event([FromBody] Event @event)
        {
            @event.TimeSpan = GetTimestamp(DateTime.Now);
            var handler = GetHandler(@event);
            handler.Handle(@event);
            return Ok();
        }

        private IHandleEvent GetHandler(Event @event)
        {
            if (@event.GetType() == typeof(Freeze))
            {
                return new FreezeEvent(ProviderConfig);
            }

            if (@event.GetType() == typeof(AddMoney))
            {
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