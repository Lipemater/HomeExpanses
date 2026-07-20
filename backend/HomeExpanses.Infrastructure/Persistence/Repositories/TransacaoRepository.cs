using HomeExpanses.Application.Abstractions.Persistence;
using HomeExpanses.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeExpanses.Infrastructure.Persistence.Repositories
{
    public sealed class TransacaoRepository : ITransacaoRepository
    {
        private readonly AppDbContext _dbContext;

        public TransacaoRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<Transacao>> ListarAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Transacoes
                .AsNoTracking()
                .Include(transacao => transacao.Pessoa)
                .OrderByDescending(transacao => transacao.Id)
                .ToListAsync(cancellationToken);
        }

        public async Task AdicionarAsync(Transacao transacao, CancellationToken cancellationToken = default)
        {
            await _dbContext.Transacoes.AddAsync(transacao, cancellationToken);
        }
    }
}