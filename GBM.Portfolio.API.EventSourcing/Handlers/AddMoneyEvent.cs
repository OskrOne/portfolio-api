using System.Collections.Generic;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using GBM.Portfolio.DataProvider;
using GBM.Portfolio.Model.Events;

namespace GBM.Portfolio.API.EventSourcing.Handlers
{
    public class AddMoneyEvent : EventHandler
    {
        public AddMoneyEvent() : base()
        {
        }

        public AddMoneyEvent(ProviderConfig config) : base(config)
        {
        }

        public AddMoneyEvent(IAmazonDynamoDB client) : base(client)
        {
        }

        public override TransactWriteItem GetInsertEvent(Event _event)
        {
            var @event = (EventAddMoney)_event;
            var transact = new TransactWriteItem()
            {
                Put = new Put()
                {
                    TableName = Constants.PortfolioEventTableName,
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

        public override TransactWriteItem GetUpdateState(Event _event)
        {
            var @event = (EventAddMoney)_event;
            var transact = new TransactWriteItem()
            {
                Update = new Update()
                {
                    TableName = Constants.PortfolioTableName,
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

        public override void Handle(Event _event)
        {
            var request = new TransactWriteItemsRequest();
            var transactWriteItems = new List<TransactWriteItem>();
            transactWriteItems.Add(GetInsertEvent(_event));
            transactWriteItems.Add(GetUpdateState(_event));
            request.TransactItems = transactWriteItems;
            var response = DbClient.TransactWriteItemsAsync(request);
            response.Wait();
        }
    }
}
