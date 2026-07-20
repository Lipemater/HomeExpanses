namespace HomeExpanses.Application.Abstractions.Persistence
{
    public sealed record TotalPessoaPersistencia(
        int PessoaId,
        string Nome,
        long TotalReceitasEmCentavos,
        long TotalDespesasEmCentavos);
}