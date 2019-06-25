using GBM.Portfolio.DataProvider;
using GBM.Portfolio.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GBM.Portfolio.DataAccess.Controllers
{
    [Route("api/contract/")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        private IConfiguration Config;

        public PositionController(IConfiguration configuration)
        {
            Config = configuration;
        }

        [HttpGet("{contractId}/position")]
        public ActionResult<Contract> Get(string contractId)
        {
            var config = new ProviderConfig()
            {
                Local =  bool.Parse(Config["AWS:Local"]),
                DynamoDBURL = Config["AWS:DynamoDBURL"],
                AwsAccessKeyId = Config["AWS:AccessKeyId"],
                AwsSecretAccessKey = Config["AWS:SecretAccessKey"],
                RegionEndpoint = Amazon.RegionEndpoint.USWest2 // TODO: Include this configuration in AppSettings
            };
            var provider = new ContractProvider(config);
            return provider.Get(contractId);
        }
    }
}
