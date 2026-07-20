using HomeExpanses.Application.DTOs.Pessoas;

namespace HomeExpanses.Application.Abstractions.Services
{
    public interface IPessoaService
    {
        Task<IReadOnlyList<PessoaResponse>> ListarAsync(CancellationToken cancellationToken = default);
        Task<PessoaResponse> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
        Task<PessoaResponse> CriarAsync(CriarPessoaRequest request, CancellationToken cancellationToken = default);
        Task ExcluirAsync(
            int id,
            CancellationToken cancellationToken = default);
    }
}