using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERPLocadoras.Core.Entities;

namespace ERPLocadoras.Infra.Data.Configurations
{
    public class LocacaoConfiguration : IEntityTypeConfiguration<Locacao>
    {
        public void Configure(EntityTypeBuilder<Locacao> builder)
        {
            builder.ToTable("Locacoes");

            builder.HasKey(l => l.Id);

            builder.Property(l => l.LocadoraId)
                .IsRequired();

            builder.Property(l => l.VeiculoId)
                .IsRequired();

            builder.Property(l => l.ClienteId)
                .IsRequired();

            // Indexes
            builder.HasIndex(l => l.LocadoraId);
            builder.HasIndex(l => l.VeiculoId);
            builder.HasIndex(l => l.ClienteId);
        }
    }
}