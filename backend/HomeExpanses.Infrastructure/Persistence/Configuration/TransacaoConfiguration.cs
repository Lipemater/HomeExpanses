using HomeExpanses.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeExpanses.Infrastructure.Persistence.Configuration
{
    public sealed class TransacaoConfiguration : IEntityTypeConfiguration<Transacao>
    {
        public void Configure(EntityTypeBuilder<Transacao> builder)
        {
            builder.ToTable("Transacoes");

            builder.HasKey(transacao => transacao.Id);

            builder.Property(transacao => transacao.Id).ValueGeneratedOnAdd();

            builder.Property(transacao => transacao.Descricao)
                .IsRequired()
                .HasMaxLength(Transacao.DescricaoMaximo);

            builder.Property(transacao => transacao.ValorEmCentavos)
                .IsRequired();

            
            /*
            * O enum será armazenado como texto:
            * "Despesa" ou "Receita".
            * Isso melhora a legibilidade do banco e evita dependência
            * dos números internos definidos no enum.
            */
            builder.Property(transacao => transacao.Tipo)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(20);
            
            builder.Ignore(transacao => transacao.Valor);
        }
    }
}