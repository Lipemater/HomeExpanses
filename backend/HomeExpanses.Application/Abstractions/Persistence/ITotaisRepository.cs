namespace HomeExpanses.Application.Abstractions.Persistence
{
    public interface ITotaisRepository
    {
        Task<IReadOnlyList<TotalPessoaPersistencia>> ConsultarAsync(CancellationToken cancellationToken = default);
    }
}