using HomeExpanses.Application.DTOs.Transacoes;

namespace HomeExpanses.Application.Abstractions.Services
{
    public interface ITransacaoService
    {
        Task<IReadOnlyList<TransacaoResponse>> ListarAsync(CancellationToken cancellationToken = default);
        Task<TransacaoResponse> CriarAsync(CriarTransacaoRequest request, CancellationToken cancellationToken = default);
    }
}