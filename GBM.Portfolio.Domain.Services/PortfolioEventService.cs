using System;
using System.Collections.Generic;
using System.Text;
using GBM.Portfolio.Domain.Models.Events;
using GBM.Portfolio.Domain.Repositories;
using GBM.Portfolio.Domain.Repositories.Events;

namespace GBM.Portfolio.Domain.Services
{
    public class PortfolioEventService : IPortfolioEventService
    {
        private readonly IPortfolioEventRepository _portfolioRepository;

        public PortfolioEventService(IPortfolioEventRepository portfolioRepository) {
            _portfolioRepository = portfolioRepository;
        }

        public List<Event> GetAll(string contracId)
        {
            return _portfolioRepository.GetAll(contracId);
        }
    }
}
