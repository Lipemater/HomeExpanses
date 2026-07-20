using System.ComponentModel.DataAnnotations;
using HomeExpanses.Domain.Enums;
using HomeExpanses.Domain.Entities;

namespace HomeExpanses.Application.DTOs.Transacoes
{
    public sealed class CriarTransacaoRequest
    {
        [Required(ErrorMessage = "A descrição da transação é obrigatória.")]
        [StringLength(Transacao.DescricaoMaximo, MinimumLength = 2, ErrorMessage = "A descrição da transação deve ter entre 2 e 100 caracteres.")]
        public string Descricao { get; init; } = string.Empty;

        [Range(
            typeof(decimal),
            "0.01",
            "999999999.99",
            ParseLimitsInInvariantCulture = true,
            ErrorMessage = "O valor da transação deve ser maior que zero.")]
        public decimal Valor { get; init; }

        [EnumDataType(typeof(TipoTransacao), ErrorMessage = "O tipo da transação é inválido.")]
        public TipoTransacao Tipo { get; init; }

        [Range(1, int.MaxValue, ErrorMessage = "O Id da pessoa deve ser maior que zero.")]
        public int PessoaId { get; init; }
    }
}
