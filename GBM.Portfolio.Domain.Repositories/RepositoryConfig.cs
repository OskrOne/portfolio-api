using Amazon.DynamoDBv2;

namespace GBM.Portfolio.Domain.Repositories
{
    public class RepositoryConfig
    {
        public bool Local { get; set; }
        public string DynamoDBURL { get; set; }
        public string AwsAccessKeyId { get; set; }
        public string AwsSecretAccessKey { get; set; }
        public Amazon.RegionEndpoint RegionEndpoint { get; set; }
    }

    public class Repository {
        public static AmazonDynamoDBClient GetAmazonDynamoDBClient(RepositoryConfig config)
        {
            AmazonDynamoDBClient dbClient;
            if (config.Local)
            {
                AmazonDynamoDBConfig clientConfig = new AmazonDynamoDBConfig
                {
                    ServiceURL = config.DynamoDBURL,
                    
                };
                dbClient = new AmazonDynamoDBClient(clientConfig);
            }
            else if (config.AwsAccessKeyId == string.Empty || config.AwsSecretAccessKey == string.Empty)
            {
                dbClient = new AmazonDynamoDBClient(config.RegionEndpoint);
            }
            else
            {
                dbClient = new AmazonDynamoDBClient(config.AwsAccessKeyId, config.AwsSecretAccessKey, config.RegionEndpoint);
            }

            return dbClient;
        }
    }
}
