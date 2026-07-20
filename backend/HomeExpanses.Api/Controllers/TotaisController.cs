using HomeExpanses.Application.Abstractions.Services;
using HomeExpanses.Application.DTOs.Totais;
using Microsoft.AspNetCore.Mvc;

namespace HomeExpanses.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class TotaisController : ControllerBase
    {
        private readonly ITotaisService _totaisService;

        public TotaisController(ITotaisService totaisService)
        {
            _totaisService = totaisService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ConsultaTotaisResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<ConsultaTotaisResponse>> Consultar(CancellationToken cancellationToken)
        {
            var totais = await _totaisService.ConsultarAsync(cancellationToken);
            return Ok(totais);
        }
    }
}