using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using GBM.Portfolio.Domain.Models;

namespace GBM.Portfolio.Domain.Repositories
{
    public class AssetRepository : BaseRepository, IAssetRepository
    {
        public AssetRepository() : base() { }

        public AssetRepository(IAmazonDynamoDB client) : base(client) { }

        public AssetRepository(RepositoryConfig config) : base(config) { }

        public List<Asset> GetAll(string contractId)
        {
            var request = new QueryRequest()
            {
                TableName = Tables.Assets,
                KeyConditionExpression = "ContractId = :ContractId",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>() {
                    { ":ContractId", new AttributeValue(){ S = contractId } }
                }
            };

            var result = _dbClient.QueryAsync(request);
            result.Wait();
            if (result.IsCompletedSuccessfully)
            {
                return GetAssets(result.Result.Items);
            }

            throw new ApplicationException("Error reading Assets information", result.Exception);
        }

        private List<Asset> GetAssets(List<Dictionary<string, AttributeValue>> items)
        {
            List<Asset> assets = new List<Asset>();
            foreach (var item in items)
            {
                var asset = new Asset();
                asset.ContractId = item["ContractId"].S;
                asset.InstrumentId = item["InstrumentId"].S;

                if (item.ContainsKey("Quantity"))
                {
                    asset.Quantity = int.Parse(item["Quantity"].N);
                }

                if (item.ContainsKey("AveragePrice"))
                {
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

            return assets;
        }
    }
}
