using HomeExpanses.Application.DTOs.Totais;

namespace HomeExpanses.Application.Abstractions.Services
{
    public interface ITotaisService
    {
        Task<ConsultaTotaisResponse> ConsultarAsync(CancellationToken cancellationToken = default);
    }
}