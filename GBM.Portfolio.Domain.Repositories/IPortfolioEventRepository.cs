using System;
using System.Collections.Generic;
using System.Text;
using GBM.Portfolio.Domain.Models.Events;

namespace GBM.Portfolio.Domain.Repositories
{
    public interface IPortfolioEventRepository
    {
        List<Event> GetAll(string contractId);
    }
}
