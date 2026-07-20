namespace HomeExpanses.Application.DTOs.Totais
{
    public sealed record TotalGeralResponse(
        decimal TotalReceitas,
        decimal TotalDespesas,
        decimal Saldo);
}