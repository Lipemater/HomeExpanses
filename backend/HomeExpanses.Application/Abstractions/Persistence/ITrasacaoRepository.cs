using HomeExpanses.Domain.Entities;

namespace HomeExpanses.Application.Abstractions.Persistence
{
    public interface ITransacaoRepository
    {
        Task<IReadOnlyList<Transacao>> ListarAsync(
            CancellationToken cancellationToken = default);
        
        Task AdicionarAsync(
            Transacao transacao,
            CancellationToken cancellationToken = default);
    }
}