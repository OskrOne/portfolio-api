using System.Collections.Generic;
using GBM.Portfolio.Domain.Models.Events;

namespace GBM.Portfolio.Domain.Services
{
    public interface IPortfolioEventService
    {
        List<Event> GetAll(string contracId);
    }
}
