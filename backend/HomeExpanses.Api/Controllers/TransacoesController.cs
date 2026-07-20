using HomeExpanses.Application.Abstractions.Services;
using HomeExpanses.Application.DTOs.Transacoes;
using Microsoft.AspNetCore.Mvc;

namespace HomeExpanses.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class TransacoesController : ControllerBase
    {
         private readonly ITransacaoService _transacaoService;

        public TransacoesController(
            ITransacaoService transacaoService)
        {
            _transacaoService = transacaoService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<TransacaoResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<TransacaoResponse>>> Listar(CancellationToken cancellationToken)
        {
            var transacoes = await _transacaoService.ListarAsync(cancellationToken);

            return Ok(transacoes);
        }

        [HttpPost]
        [ProducesResponseType(typeof(TransacaoResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TransacaoResponse>> Criar([FromBody] CriarTransacaoRequest request, CancellationToken cancellationToken)
        {
            var trasacao = await _transacaoService.CriarAsync(request, cancellationToken);

            return StatusCode(StatusCodes.Status201Created, trasacao);
        }
    }
}
