using System.Collections.Generic;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using GBM.Portfolio.Domain.Models.Events;
using GBM.Portfolio.Domain.Repositories;

namespace GBM.Portfolio.Domain.Repositories.Events
{
    public class FreezeEvent : BaseRepository, IHandleEvent
    {
        public FreezeEvent() : base() { }

        public FreezeEvent(RepositoryConfig config) : base(config) { }

        public FreezeEvent(IAmazonDynamoDB client) : base(client) { }

        private TransactWriteItem GetInsertEvent(Event _event)
        {
            var @event = (Models.Events.Freeze)_event;
            var transact = new TransactWriteItem()
            {
                Put = new Put()
                {
                    TableName = Tables.PortfolioEvents,
                    Item = new Dictionary<string, AttributeValue>() {
                        { "ContractId", new AttributeValue() { S = @event.ContractId } },
                        { "TimeStamp", new AttributeValue(){ S = @event.TimeSpan.ToString() } },
                        { "EventType", new AttributeValue(){ S = @event.EventType.ToString() } },
                        { "InstrumentId", new AttributeValue() { N = @event.InstrumentId.ToString() } },
                        { "InstrumentName", new AttributeValue() { S = @event.InstrumentName } },
                        { "Quantity", new AttributeValue() { N = @event.Quantity.ToString() } },
                        { "Price", new AttributeValue() { N = @event.Price.ToString() } },
                        { "Side", new AttributeValue() { S = @event.Side.ToString() } },
                    }
                }
            };

            return transact;
        }

        private TransactWriteItem GetUpdateState(Event _event)
        {
            var @event = (Models.Events.Freeze)_event;
            var transact = new TransactWriteItem()
            {
                Update = new Update()
                {
                    TableName = Tables.Portfolio,
                    Key = new Dictionary<string, AttributeValue>() {
                        { "ContractId", new AttributeValue() { S = @event.ContractId } }
                    },
                    UpdateExpression = "set BuyingPower = BuyingPower - :total",
                    ConditionExpression = "BuyingPower >= :total",
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>() {
                        { ":total", new AttributeValue(){ N = (@event.Price * @event.Quantity).ToString() } }
                    }
                }
            };

            return transact;
        }

        public void Handle(Event _event)
        {
            TransactWriteItemsRequest request = new TransactWriteItemsRequest();
            List<TransactWriteItem> transactWriteItems = new List<TransactWriteItem>();

            transactWriteItems.Add(GetInsertEvent(_event));
            transactWriteItems.Add(GetUpdateState(_event));
            request.TransactItems = transactWriteItems;

            var response = _dbClient.TransactWriteItemsAsync(request);
            response.Wait();

            // TODO: Handle exceptions
        }
    }
}
