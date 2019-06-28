using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace GBM.Portfolio.Domain.Repositories
{
    public class PortfolioRepository : BaseRepository, IPortfolioRepository
    {
        private readonly string _tableName = "Portfolio";

        public PortfolioRepository() : base() { }

        public PortfolioRepository(IAmazonDynamoDB client) : base(client) { }

        public PortfolioRepository(RepositoryConfig config) : base(config) { }

        public Domain.Models.Portfolio Get(string contractId)
        {
            var request = new GetItemRequest()
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>()
                {
                    { "ContractId", new AttributeValue(){ S = contractId } }
                }
            };

            var result = _dbClient.GetItemAsync(request);
            result.Wait();

            if (result.IsCompletedSuccessfully)
            {
                var contract = GetContract(result.Result.Item);
                var assetProvider = new AssetRepository(_dbClient);
                contract.Assets = assetProvider.GetAll(contract.ContractId).ToArray();
                return contract;
            }

            throw new ApplicationException("Error reading Contract information", result.Exception);
        }

        private Domain.Models.Portfolio GetContract(Dictionary<string, AttributeValue> item)
        {
            // Maybe we should implement a mapper
            var contract = new Models.Portfolio();
            contract.ContractId = item["ContractId"].S;

            if (item.ContainsKey("BuyingPower"))
            {
                contract.BuyingPower = Decimal.Parse(item["BuyingPower"].N);
            }

            return contract;
        }

        private void SetupDatabase()
        {
            var result = _dbClient.ListTablesAsync();
            result.Wait();
            var tables = result.Result;

            if (tables.TableNames.Count == 0)
            {
                CreateTables();
                InsertIntoTables();
            }
        }

        private void InsertIntoTables()
        {
            var request = new PutItemRequest()
            {
                TableName = "Contract",
                Item = new Dictionary<string, AttributeValue>() {
                    { "ContractId", new AttributeValue("A100") },
                    { "BuyingPower", new AttributeValue(){  N = "10000" } }
                }
            };

            var result = _dbClient.PutItemAsync(request);
            result.Wait();

            request = new PutItemRequest()
            {
                TableName = "Assets",
                Item = new Dictionary<string, AttributeValue>() {
                    { "ContractId", new AttributeValue(){ S = "A100" } },
                    { "InstrumentId", new AttributeValue(){ N = "1003232" } },
                    { "Quantity", new AttributeValue(){ N = "10" } },
                    { "AveragePrice", new AttributeValue(){ N = "1000" } },
                    { "LastPrice", new AttributeValue(){ N = "1000" } },
                    { "ClosePrice", new AttributeValue(){ N = "1000" } }
                }
            };

            result = _dbClient.PutItemAsync(request);
            result.Wait();
        }

        private void CreateTables()
        {
            var request = new CreateTableRequest()
            {
                TableName = "Contract",
                AttributeDefinitions = new List<AttributeDefinition>() {
                        new AttributeDefinition("ContractId", ScalarAttributeType.S)
                    },
                KeySchema = new List<KeySchemaElement>() {
                        new KeySchemaElement("ContractId", KeyType.HASH)
                    },
                ProvisionedThroughput = new ProvisionedThroughput(5, 5)
            };

            var resultCreateTable = _dbClient.CreateTableAsync(request);
            resultCreateTable.Wait();

            request = new CreateTableRequest()
            {
                TableName = "Assets",
                AttributeDefinitions = new List<AttributeDefinition>() {
                        new AttributeDefinition("ContractId", ScalarAttributeType.S)
                    },
                KeySchema = new List<KeySchemaElement>() {
                        new KeySchemaElement("ContractId", KeyType.HASH)
                    },
                ProvisionedThroughput = new ProvisionedThroughput(5, 5)
            };

            resultCreateTable = _dbClient.CreateTableAsync(request);
            resultCreateTable.Wait();
        }
    }
}
