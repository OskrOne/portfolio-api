using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GBM.Portfolio.Domain.Models.Events;
using GBM.Portfolio.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GBM.Portfolio.API.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioEventController : ControllerBase
    {
        private readonly IPortfolioEventService _portfolioEventService;

        public PortfolioEventController(IPortfolioEventService portfolioEventService) {
            _portfolioEventService = portfolioEventService;
        }

        [HttpGet("{contractId}/events")]
        public ActionResult<Event> GetAll(string contractId) {
            var events = _portfolioEventService.GetAll(contractId);
            return Ok(events);
        }
    }
}