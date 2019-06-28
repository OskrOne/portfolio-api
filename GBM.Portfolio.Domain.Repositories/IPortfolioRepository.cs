using GBM.Portfolio.Domain.Models;

namespace GBM.Portfolio.Domain.Repositories
{
    public interface IPortfolioRepository
    {
        Domain.Models.Portfolio Get(string contractId);
    }
}
