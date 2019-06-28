using System;
using System.Collections.Generic;
using System.Text;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using GBM.Portfolio.Domain.Models.Events;

namespace GBM.Portfolio.Domain.Repositories
{
    public class PortfolioEventRepository : BaseRepository, IPortfolioEventRepository
    {
        public PortfolioEventRepository() : base() { }

        public PortfolioEventRepository(IAmazonDynamoDB client) : base(client) { }

        public PortfolioEventRepository(RepositoryConfig config) : base(config) { }

        public List<Event> GetAll(string contractId)
        {
            var request = new QueryRequest()
            {
                TableName = Tables.PortfolioEvents,
                KeyConditionExpression = "ContractId = :ContractId",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>() {
                    { ":ContractId", new AttributeValue(){ S = contractId } }
                }
            };

            var result = _dbClient.QueryAsync(request);
            result.Wait();
            if (result.IsCompletedSuccessfully)
            {
                return GetPortfolioEvents(result.Result.Items);
            }

            throw new ApplicationException("Error reading Portfolio events", result.Exception);
        }

        private List<Event> GetPortfolioEvents(List<Dictionary<string, AttributeValue>> items)
        {
            var events = new List<Event>();
            foreach (var item in items)
            {
                if (item["EventType"].S == EventType.AddMoney.ToString())
                {
                    events.Add(new AddMoney()
                    {
                        ContractId = item["ContractId"].S,
                        EventType = (EventType)Enum.Parse(typeof(EventType), item["EventType"].S),
                        Money = Decimal.Parse(item["Money"].N),
                        TimeSpan = item["TimeStamp"].S
                    });
                }
                else {
                    events.Add(new Freeze()
                    {
                        ContractId = item["ContractId"].S,
                        EventType = (EventType)Enum.Parse(typeof(EventType), item["EventType"].S),
                        InstrumentId = item["InstrumentId"].N,
                        InstrumentName = item["InstrumentName"].S,
                        Price = Decimal.Parse(item["Price"].N),
                        Quantity = int.Parse(item["Quantity"].N),
                        Side = (Side)Enum.Parse(typeof(Side), item["Side"].S),
                        TimeSpan = item["TimeStamp"].S
                    });
                }
            }

            return events;
        }
    }
}
