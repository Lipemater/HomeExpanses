using HomeExpanses.Domain.Entities;

namespace HomeExpanses.Application.Abstractions.Persistence
{
    public interface IPessoaRepository
    {
        Task<IReadOnlyList<Pessoa>> ListarAsync(
            CancellationToken cancellationToken = default);
        
        Task<Pessoa?> ObterPorIdAsync(
            int id, 
            CancellationToken cancellationToken = default);
        
        Task AdicionarAsync(
            Pessoa pessoa,
            CancellationToken cancellationToken = default);

        void Remover(Pessoa pessoa);

        Task ReordenarIdsAsync(
            CancellationToken cancellationToken = default);
    }
}