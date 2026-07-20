using System.ComponentModel.DataAnnotations;
using HomeExpanses.Domain.Entities;

namespace HomeExpanses.Application.DTOs.Pessoas
{
    public sealed class CriarPessoaRequest
    {
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [StringLength(Pessoa.NomeMaximo, MinimumLength = 2, ErrorMessage = "O campo Nome deve ter entre 2 e 100 caracteres.")]
        public string Nome { get; init; } = string.Empty;

        [Range(0, 110, ErrorMessage = "O campo Idade deve estar entre 0 e 110 anos.")]
        public int Idade { get; init; }
    
    }
}