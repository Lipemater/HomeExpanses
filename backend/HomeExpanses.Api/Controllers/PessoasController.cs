using HomeExpanses.Application.Abstractions.Services;
using HomeExpanses.Application.DTOs.Pessoas;
using Microsoft.AspNetCore.Mvc;

namespace HomeExpanses.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class PessoasController  : ControllerBase
    {
        private readonly IPessoaService _pessoaService;

        public PessoasController(IPessoaService pessoaService)
        {
            _pessoaService = pessoaService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<PessoaResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<PessoaResponse>>> Listar(CancellationToken cancellationToken)
        {
            var pessoas = await _pessoaService.ListarAsync(cancellationToken);
            return Ok(pessoas);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(PessoaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PessoaResponse>> ObterPorId(int id, CancellationToken cancellationToken)
        {
            var pessoa = await _pessoaService.ObterPorIdAsync(id, cancellationToken);
            return Ok(pessoa);
        }


        [HttpPost]
        [ProducesResponseType(typeof(PessoaResponse), StatusCodes.Status201Created)]
        public async Task<ActionResult<PessoaResponse>> Criar([FromBody] CriarPessoaRequest request, CancellationToken cancellationToken)
        {
            var pessoa = await _pessoaService.CriarAsync(request, cancellationToken);

            return CreatedAtAction(nameof(ObterPorId), new { id = pessoa.Id }, pessoa);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Excluir(int id, CancellationToken cancellationToken)
        {
            await _pessoaService.ExcluirAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
