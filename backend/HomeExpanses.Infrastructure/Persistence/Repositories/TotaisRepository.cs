using HomeExpanses.Application.Abstractions.Persistence;
using HomeExpanses.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace HomeExpanses.Infrastructure.Persistence.Repositories
{
    public sealed class TotaisRepository : ITotaisRepository
    {
        private readonly AppDbContext _dbContext;

        public TotaisRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<TotalPessoaPersistencia>> ConsultarAsync(CancellationToken cancellationToken = default)
        {
            /*
            * Os cálculos são executados no banco.
            *
            * A conversão para reais é feita posteriormente pela
            * Application, mantendo as somas em centavos inteiros.
            */

            return await _dbContext.Pessoas
                .AsNoTracking()
                .OrderBy(pessoa => pessoa.Nome)
                .ThenBy(pessoa => pessoa.Id)
                .Select(pessoa => new TotalPessoaPersistencia(pessoa.Id, pessoa.Nome,

                    pessoa.Transacoes
                        .Where(transacao => transacao.Tipo == TipoTransacao.Receita)
                        .Sum(transacao => (long?)transacao.ValorEmCentavos) ??0L, 
                    
                    pessoa.Transacoes
                        .Where(transacao => transacao.Tipo == TipoTransacao.Despesa)
                        .Sum(transacao => (long?)transacao.ValorEmCentavos) ??0L)).ToListAsync(cancellationToken);
        }
    }
}