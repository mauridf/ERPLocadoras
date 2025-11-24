using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERPLocadoras.Core.Entities;

namespace ERPLocadoras.Infra.Data.Configurations
{
    public class LocadoraConfiguration : IEntityTypeConfiguration<Locadora>
    {
        public void Configure(EntityTypeBuilder<Locadora> builder)
        {
            builder.ToTable("Locadoras");

            builder.HasKey(l => l.Id);

            builder.Property(l => l.RazaoSocial)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(l => l.NomeFantasia)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(l => l.CNPJ)
                .IsRequired()
                .HasMaxLength(14);

            builder.Property(l => l.InscricaoEstadual)
                .HasMaxLength(20);

            builder.Property(l => l.InscricaoMunicipal)
                .HasMaxLength(20);

            builder.Property(l => l.CNAEPrincipal)
                .HasMaxLength(10);

            builder.Property(l => l.RegimeTributario)
                .HasMaxLength(50);

            builder.Property(l => l.Status)
                .IsRequired()
                .HasConversion<int>();

            // Indexes
            builder.HasIndex(l => l.CNPJ)
                .IsUnique();

            builder.HasIndex(l => l.EmailComercial);

            // Relationships
            builder.HasMany(l => l.Usuarios)
                .WithOne(u => u.Locadora)
                .HasForeignKey(u => u.LocadoraId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(l => l.Veiculos)
                .WithOne(v => v.Locadora)
                .HasForeignKey(v => v.LocadoraId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}