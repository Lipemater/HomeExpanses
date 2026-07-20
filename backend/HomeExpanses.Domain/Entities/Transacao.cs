using HomeExpanses.Domain.Enums;
using HomeExpanses.Domain.Exceptions;

namespace HomeExpanses.Domain.Entities
{
    public sealed class Transacao
    {
        public const int DescricaoMaximo = 200;

        /*
        * Construtor sem parâmetros utilizado pelo Entity Framework.
        * Ele é privado para impedir a criação de transações inválidas
        * fora da própria entidade.
        */
        private Transacao()
        {
        }

        /*
        * O metodo Transacao é privado para que nenhuma outra camada possa criar transação sem que passe pelo método Criar.
        * O método Criar foi feito para validar a criação de uma nova Transação.
        */
        private Transacao (
            Pessoa pessoa,
            string descricao,
            decimal valor,
            TipoTransacao tipo)
        {
            Pessoa = pessoa;
            PessoaId = pessoa.Id;
            Descricao = ValidarDescricao(descricao);
            Tipo = ValidarTipo(tipo);
            ValorEmCentavos = ConverterParaCentavos(valor);
        }
        public int Id { get; private set; }
        public string Descricao { get; private set; } = string.Empty;

        /*
        * Dinheiro é persistido como número inteiro de centavos.
        * Isso evita perda de precisão e mantém a representação compatível com o SQLite.
        * Exemplo:
        * R$ 10,50 é armazenado como 1050.
        */
        public long ValorEmCentavos { get; private set; }
        public TipoTransacao Tipo { get; private set; }
        public int PessoaId { get; private set; }
        public Pessoa Pessoa { get; private set; } = null!;

        /*
        * Esta propriedade é utilizada pela aplicação para devolver
        * o valor em reais. Ela não será armazenada como uma coluna.
        */
        public decimal Valor => ValorEmCentavos / 100m;


        public static Transacao Criar(
            Pessoa pessoa,
            string descricao,
            decimal valor,
            TipoTransacao tipo)
        {
            ArgumentNullException.ThrowIfNull(pessoa);
            
            if (pessoa.MenorDeIdade() && tipo == TipoTransacao.Receita)
            {
                throw new RegraNegocioException("Pessoas menores de idade não podem ter receitas.");
            }

            return new Transacao(pessoa, descricao, valor, tipo); 
        }

        private static string ValidarDescricao(string descricao)
        {
            if (string.IsNullOrWhiteSpace(descricao))
            {
                throw new RegraNegocioException("A descrição da transação não pode ser vazia.");
            }

            var descricaoTrimmed = descricao.Trim();
            if (descricaoTrimmed.Length > DescricaoMaximo)
            {
                throw new RegraNegocioException($"A descrição da transação não pode ter mais de {DescricaoMaximo} caracteres.");
            }

            return descricaoTrimmed;
        }

        private static TipoTransacao ValidarTipo(TipoTransacao tipo)
        {
            if (!Enum.IsDefined(typeof(TipoTransacao), tipo))
            {
                throw new RegraNegocioException("Tipo de transação inválido. Deve ser 'Despesa' ou 'Receita'.");
        }

            return tipo;
        }

        private static long ConverterParaCentavos(decimal valor)
        {
            if (valor <= 0)
            {
                throw new RegraNegocioException("O valor da transação tem que ser maior que zero.");
            }

            try
            {
                return checked((long)decimal.Round(valor * 100m, 0, MidpointRounding.AwayFromZero));
            }
            catch (OverflowException)
            {
                throw new RegraNegocioException("O valor da transação é muito grande.");
            }
        }
    }
}
