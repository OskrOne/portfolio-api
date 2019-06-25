using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using GBM.Portfolio.Model;

namespace GBM.Portfolio.DataProvider
{
    public class ContractProvider
    {
        private readonly AmazonDynamoDBClient DbClient;
        private readonly string ContractTableName = "Contract";

        public ContractProvider() {
            DbClient = new AmazonDynamoDBClient();
        }

        public ContractProvider(ProviderConfig config) {
            DbClient = Provider.GetAmazonDynamoDBClient(config);
        }

        public ContractProvider(AmazonDynamoDBClient client) {
            DbClient = client;
        }

        public Contract Get(string contractId) {

            SetupDatabase();

            var request = new GetItemRequest()
            {
                TableName = ContractTableName,
                Key = new Dictionary<string, AttributeValue>()
                {
                    { "ContractId", new AttributeValue(){ S = contractId } }
                }
            };

            var result = DbClient.GetItemAsync(request);
            result.Wait();

            if (result.IsCompletedSuccessfully) {
                var contract = GetContract(result.Result.Item);
                var assetProvider = new AssetProvider(DbClient);
                contract.Assets = assetProvider.GetAssets(contract.ContractId);
                return contract;
            }

            throw new ApplicationException("Error reading Contract information", result.Exception);
        }

        private Contract GetContract(Dictionary<string, AttributeValue> item)
        {
            var contract = new Contract();
            contract.ContractId = item["ContractId"].S;

            if (item.ContainsKey("BuyingPower")) {
                contract.BuyingPower = Decimal.Parse(item["BuyingPower"].N);
            }

            return contract;
        }

        private void SetupDatabase()
        {
            var result = DbClient.ListTablesAsync();
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
            var request = new PutItemRequest() {
                TableName = "Contract",
                Item = new Dictionary<string, AttributeValue>() {
                    { "ContractId", new AttributeValue("A100") },
                    { "BuyingPower", new AttributeValue(){  N = "10000" } }
                }
            };

            var result = DbClient.PutItemAsync(request);
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

            result = DbClient.PutItemAsync(request);
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

            var resultCreateTable = DbClient.CreateTableAsync(request);
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

            resultCreateTable = DbClient.CreateTableAsync(request);
            resultCreateTable.Wait();
        }
    }
}
