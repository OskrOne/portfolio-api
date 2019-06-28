using System;
using System.Collections.Generic;
using System.Text;
using Amazon.DynamoDBv2;

namespace GBM.Portfolio.Domain.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly IAmazonDynamoDB _dbClient;

        public BaseRepository()
        {
            _dbClient = new AmazonDynamoDBClient();
        }

        public BaseRepository(RepositoryConfig config)
        {
            _dbClient = Repository.GetAmazonDynamoDBClient(config);
        }

        public BaseRepository(IAmazonDynamoDB client)
        {
            _dbClient = client;
        }
    }
}
