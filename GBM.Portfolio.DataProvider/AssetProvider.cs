using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using GBM.Portfolio.Model;

namespace GBM.Portfolio.DataProvider
{
    class AssetProvider
    {
        private readonly IAmazonDynamoDB DbClient;
        private readonly string AssetTableName = "Assets";

        public AssetProvider()
        {
            DbClient = new AmazonDynamoDBClient();
        }

        public AssetProvider(ProviderConfig config)
        {
            DbClient = Provider.GetAmazonDynamoDBClient(config);
        }

        public AssetProvider(IAmazonDynamoDB client)
        {
            DbClient = client;
        }

        public Asset[] GetAssets(string contractId) {
            var request = new QueryRequest()
            {
                TableName = AssetTableName,
                KeyConditionExpression = "ContractId = :ContractId",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>() {
                    { ":ContractId", new AttributeValue(){ S = contractId } }
                }
            };

            var result = DbClient.QueryAsync(request);
            result.Wait();
            if (result.IsCompletedSuccessfully) {
                return GetAssets(result.Result.Items);
            }

            throw new ApplicationException("Error reading Assets information", result.Exception);
        }

        private Asset[] GetAssets(List<Dictionary<string, AttributeValue>> items)
        {
            List<Asset> assets = new List<Asset>();
            foreach (var item in items) {
                var asset = new Asset();
                asset.ContractId = item["ContractId"].S;
                asset.InstrumentId = item["InstrumentId"].S;

                if (item.ContainsKey("Quantity")) {
                    asset.Quantity = int.Parse(item["Quantity"].N);
                }

                if (item.ContainsKey("AveragePrice")) {
                    asset.AveragePrice = Decimal.Parse(item["AveragePrice"].N);
                }

                if (item.ContainsKey("LastPrice"))
                {
                    asset.LastPrice = Decimal.Parse(item["LastPrice"].N);
                }

                if (item.ContainsKey("ClosePrice"))
                {
                    asset.ClosePrice = Decimal.Parse(item["ClosePrice"].N);
                }

                assets.Add(asset);
            }

            return assets.ToArray();
        }

        public Asset GetAsset(string contractId, string instrumentId) {
            throw new NotImplementedException();
        }
    }
}
