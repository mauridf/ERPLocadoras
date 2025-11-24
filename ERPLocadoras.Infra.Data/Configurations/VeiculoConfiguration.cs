using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERPLocadoras.Core.Entities;

namespace ERPLocadoras.Infra.Data.Configurations
{
    public class VeiculoConfiguration : IEntityTypeConfiguration<Veiculo>
    {
        public void Configure(EntityTypeBuilder<Veiculo> builder)
        {
            builder.ToTable("Veiculos");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.LocadoraId)
                .IsRequired();

            // Indexes
            builder.HasIndex(v => v.LocadoraId);
        }
    }
}