namespace GBM.Portfolio.Domain.Services
{
    public interface IPortfolioService
    {
        Domain.Models.Portfolio Get(string contractId);
    }
}
