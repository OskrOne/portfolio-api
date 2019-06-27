using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using GBM.Portfolio.DataProvider;
using GBM.Portfolio.Model.Events;

namespace GBM.Portfolio.API.EventSourcing.Handlers
{
    public abstract class EventHandler
    {
        public readonly IAmazonDynamoDB DbClient;

        public EventHandler()
        {

            DbClient = new AmazonDynamoDBClient();
        }

        public EventHandler(ProviderConfig config)
        {
            DbClient = Provider.GetAmazonDynamoDBClient(config);
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
