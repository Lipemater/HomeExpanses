namespace HomeExpanses.Application.DTOs.Totais
{
    public sealed record ConsultaTotaisResponse(
        IReadOnlyList<TotalPessoaResponse> Pessoas, TotalGeralResponse TotalGeral);
}