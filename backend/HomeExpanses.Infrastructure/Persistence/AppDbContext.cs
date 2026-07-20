using HomeExpanses.Application.Abstractions.Persistence;
using HomeExpanses.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeExpanses.Infrastructure.Persistence
{
    public sealed class AppDbContext : DbContext, IUnitOfWork
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Pessoa> Pessoas => Set<Pessoa>();
        public DbSet<Transacao> Transacoes => Set<Transacao>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}