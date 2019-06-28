using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using GBM.Portfolio.Domain.Models.Events;
using GBM.Portfolio.Domain.Repositories;

namespace GBM.Portfolio.API.EventSourcing.Handlers
{
    public abstract class EventHandler
    {
        public readonly IAmazonDynamoDB DbClient;

        public EventHandler()
        {

            DbClient = new AmazonDynamoDBClient();
        }

        public EventHandler(RepositoryConfig config)
        {
            DbClient = Repository.GetAmazonDynamoDBClient(config);
        }

        public EventHandler(IAmazonDynamoDB client)
        {
            DbClient = client;
        }

        public abstract void Handle(Event _event);
        public abstract TransactWriteItem GetInsertEvent(Event _event);
        public abstract TransactWriteItem GetUpdateState(Event _event);
    }
}
