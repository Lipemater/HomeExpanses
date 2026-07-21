using HomeExpanses.Application.Abstractions.Persistence;
using HomeExpanses.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeExpanses.Infrastructure.Persistence.Repositories
{
    public sealed class PessoaRepository : IPessoaRepository
    {
        private readonly AppDbContext _dbContext;

        public PessoaRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<Pessoa>> ListarAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Pessoas
                .AsNoTracking()
                .OrderBy(pessoa => pessoa.Nome)
                .ThenBy(Pessoa => Pessoa.Id)
                .ToListAsync(cancellationToken);
        }

        public async Task<Pessoa?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Pessoas
                .FirstOrDefaultAsync(pessoa => pessoa.Id == id, cancellationToken);
        }

        public async Task AdicionarAsync(Pessoa pessoa, CancellationToken cancellationToken = default)
        {
            await _dbContext.Pessoas.AddAsync(pessoa, cancellationToken);
        }

        public void Remover(Pessoa pessoa)
        {
            _dbContext.Pessoas.Remove(pessoa);
        }

        public async Task ReordenarIdsAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.Database.ExecuteSqlRawAsync(
                """
                WITH Numeradas AS (
                    SELECT Id,
                           ROW_NUMBER() OVER (ORDER BY Id) AS NovoId
                    FROM Pessoas
                )
                UPDATE Pessoas
                SET Id = (
                    SELECT NovoId
                    FROM Numeradas
                    WHERE Numeradas.Id = Pessoas.Id
                );

                WITH Numeradas AS (
                    SELECT Id,
                           ROW_NUMBER() OVER (ORDER BY Id) AS NovoId
                    FROM Pessoas
                )
                UPDATE Transacoes
                SET PessoaId = (
                    SELECT NovoId
                    FROM Numeradas
                    WHERE Numeradas.Id = Transacoes.PessoaId
                );

                DELETE FROM sqlite_sequence
                WHERE name = 'Pessoas';

                INSERT INTO sqlite_sequence(name, seq)
                SELECT 'Pessoas', COALESCE(MAX(Id), 0)
                FROM Pessoas;
                """,
                cancellationToken);
        }
    }
}