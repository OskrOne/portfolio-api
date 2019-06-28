using System.Collections.Generic;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using GBM.Portfolio.Domain.Models.Events;
using GBM.Portfolio.Domain.Repositories;

namespace GBM.Portfolio.Domain.Repositories.Events
{
    public class AddMoneyEvent : BaseRepository, IHandleEvent
    {
        public AddMoneyEvent() : base() { }

        public AddMoneyEvent(RepositoryConfig config) : base(config) { }

        public AddMoneyEvent(IAmazonDynamoDB client) : base(client) { }

        private TransactWriteItem GetInsertEvent(Event _event)
        {
            var @event = (Models.Events.AddMoney)_event;
            var transact = new TransactWriteItem()
            {
                Put = new Put()
                {
                    TableName = Tables.PortfolioEvents,
                    Item = new Dictionary<string, AttributeValue>() {
                        { "ContractId", new AttributeValue() { S = @event.ContractId } },
                        { "TimeStamp", new AttributeValue(){ S = @event.TimeSpan.ToString() } },
                        { "EventType", new AttributeValue(){ S = @event.EventType.ToString() } },
                        { "Money", new AttributeValue(){ N = @event.Money.ToString() } }
                    }
                }
            };

            return transact;
        }

        private TransactWriteItem GetUpdateState(Event _event)
        {
            var @event = (Models.Events.AddMoney)_event;
            var transact = new TransactWriteItem()
            {
                Update = new Update()
                {
                    TableName = Tables.Portfolio,
                    Key = new Dictionary<string, AttributeValue>() {
                        { "ContractId", new AttributeValue(){  S = @event.ContractId} }
                    },
                    UpdateExpression = "set BuyingPower = BuyingPower + :money",
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>() {
                        { ":money", new AttributeValue(){ N = @event.Money.ToString() } }
                    }
                }
            };

            return transact;
        }

        public void Handle(Event @event)
        {
            var request = new TransactWriteItemsRequest();
            var transactWriteItems = new List<TransactWriteItem>
            {
                GetInsertEvent(@event),
                GetUpdateState(@event)
            };
            request.TransactItems = transactWriteItems;
            var response = _dbClient.TransactWriteItemsAsync(request);
            response.Wait();
        }
    }
}
