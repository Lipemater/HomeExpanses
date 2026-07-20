namespace HomeExpanses.Application.DTOs.Totais
{
    public sealed record TotalPessoaResponse(
        int PessoaId,
        string Nome,
        decimal TotalReceitas,
        decimal TotalDespesas,
        decimal Saldo);
}