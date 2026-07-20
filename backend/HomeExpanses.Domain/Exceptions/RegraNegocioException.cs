// Criação de uma exceção à parte de Exception, para facilitar as tratativas de erros. 
// Essa classe, separa os erros inesperados dos erros de entradas inválidas
namespace HomeExpanses.Domain.Exceptions
{
    public sealed class RegraNegocioException : Exception
    {
        public RegraNegocioException(string message) : base(message)
        {
        }
    }
}