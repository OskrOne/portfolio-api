using GBM.Portfolio.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using GBM.Portfolio.Domain.Services;

namespace GBM.Portfolio.DataAccess.Controllers
{
    [Route("api/portfolio/")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        private readonly IPortfolioService _portfolioService;

        public PositionController(IPortfolioService portfolioService) {
            _portfolioService = portfolioService;
        }

        [HttpGet("{contractId}/position")]
        public ActionResult<Domain.Models.Portfolio> Get(string contractId)
        {
            var contract = _portfolioService.Get(contractId);
            return Ok(contract);
        }
    }
}
