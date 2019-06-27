using GBM.Portfolio.DataProvider;
using GBM.Portfolio.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GBM.Portfolio.DataAccess.Controllers
{
    [Route("api/portfolio/")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        private ProviderConfig ProviderConfig;

        public PositionController(IConfiguration configuration)
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

        [HttpGet("{contractId}/position")]
        public ActionResult<Contract> Get(string contractId)
        {
            var provider = new PortfolioProvider(ProviderConfig);
            return provider.Get(contractId);
        }
    }
}
