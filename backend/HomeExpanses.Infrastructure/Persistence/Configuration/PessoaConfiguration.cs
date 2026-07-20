using HomeExpanses.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeExpanses.Infrastructure.Persistence.Configuration
{
    public sealed class PessoaConfiguration : IEntityTypeConfiguration<Pessoa>
    {
        public void Configure(EntityTypeBuilder<Pessoa> builder)
        {
            builder.ToTable("Pessoas");

            builder.HasKey(pessoa => pessoa.Id);

            builder.Property(pessoa => pessoa.Id).ValueGeneratedOnAdd();

            builder.Property(pessoa => pessoa.Nome)
                .IsRequired()
                .HasMaxLength(Pessoa.NomeMaximo);

            builder.Property(pessoa => pessoa.Idade)
                .IsRequired();

            builder.HasMany(pessoa => pessoa.Transacoes)
                .WithOne(transacao => transacao.Pessoa)
                .HasForeignKey(transacao => transacao.PessoaId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}