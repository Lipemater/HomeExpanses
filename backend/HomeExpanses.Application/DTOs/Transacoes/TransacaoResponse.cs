using HomeExpanses.Domain.Enums;

namespace HomeExpanses.Application.DTOs.Transacoes
{
    public sealed record TransacaoResponse(
        int Id,
        string Descricao,
        decimal Valor,
        TipoTransacao Tipo,
        int PessoaId,
        string PessoaNome);
}