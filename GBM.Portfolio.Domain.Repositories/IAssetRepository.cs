using System.Collections.Generic;
using GBM.Portfolio.Domain.Models;

namespace GBM.Portfolio.Domain.Repositories
{
    interface IAssetRepository
    {
        List<Asset> GetAll(string contractId);
    }
}
