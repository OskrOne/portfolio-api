using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GBM.Portfolio.Domain.Models;
using GBM.Portfolio.Domain.Repositories;

namespace GBM.Portfolio.Domain.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly IPortfolioRepository _portfolioRepository;

        public PortfolioService(IPortfolioRepository portfolioRepository) {
            _portfolioRepository = portfolioRepository;
        }

        public Models.Portfolio Get(string contractId)
        {
            return _portfolioRepository.Get(contractId);
        }
    }
}
