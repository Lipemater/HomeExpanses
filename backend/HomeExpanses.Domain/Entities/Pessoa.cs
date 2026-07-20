using HomeExpanses.Domain.Exceptions;

namespace HomeExpanses.Domain.Entities
{
    public sealed class Pessoa
    {
        public const int NomeMaximo = 100;

        /*
        * Construtor sem parâmetros utilizado pelo Entity Framework.
        * Ele é privado para impedir a criação de pessoas inválidas
        * fora da própria entidade.
        */
        private Pessoa()
        {
        }

        public Pessoa(string nome, int idade)
        {
            AlterarNome(nome);
            DefinirIdade(idade);
        }

        public int Id { get; private set; }
        public string Nome { get; private set; } = string.Empty;
        public int Idade { get; private set; }
        public ICollection<Transacao> Transacoes { get; private set; } = new List<Transacao>();
        
        public bool MenorDeIdade()
        {
            return Idade < 18;
        }

        private void AlterarNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                throw new RegraNegocioException("O nome da pessoa não pode ser vazio.");
            }

            var nomeTrimmed = nome.Trim();
            if (nomeTrimmed.Length > NomeMaximo)
            {
                throw new RegraNegocioException($"O nome da pessoa não pode ter mais de {NomeMaximo} caracteres.");
            }

            Nome = nomeTrimmed;
        }

        private void DefinirIdade(int idade)
        {
            if (idade < 0 || idade > 110)
            {
                throw new RegraNegocioException("A idade da pessoa deve estar entre 0 e 110 anos.");
            }

            Idade = idade;
        }
    }
}