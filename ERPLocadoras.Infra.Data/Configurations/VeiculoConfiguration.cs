using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERPLocadoras.Core.Entities;
using ERPLocadoras.Core.Enums;

namespace ERPLocadoras.Infra.Data.Configurations
{
    public class VeiculoConfiguration : IEntityTypeConfiguration<Veiculo>
    {
        public void Configure(EntityTypeBuilder<Veiculo> builder)
        {
            builder.ToTable("Veiculos");

            builder.HasKey(v => v.Id);

            // Configurações básicas
            builder.Property(v => v.Tipo)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(v => v.Marca)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(v => v.Modelo)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(v => v.VersaoMotorizacao)
                .HasMaxLength(100);

            builder.Property(v => v.AnoFabricacao)
                .IsRequired();

            builder.Property(v => v.AnoModelo)
                .IsRequired();

            builder.Property(v => v.Placa)
                .IsRequired()
                .HasMaxLength(8);

            builder.Property(v => v.Renavam)
                .IsRequired()
                .HasMaxLength(11);

            builder.Property(v => v.Chassi)
                .IsRequired()
                .HasMaxLength(17);

            builder.Property(v => v.Cor)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(v => v.Categoria)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(v => v.Combustivel)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(v => v.QuilometragemAtual)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(v => v.Capacidade)
                .HasMaxLength(50);

            builder.Property(v => v.Status)
                .IsRequired()
                .HasConversion<int>();

            // Configurações de aquisição
            builder.Property(v => v.DataAquisicao)
                .IsRequired();

            builder.Property(v => v.ValorCompra)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(v => v.ValorMercadoAtual)
                .HasColumnType("decimal(10,2)");

            // Configurações de seguro
            builder.Property(v => v.ApoliceSeguro)
                .HasMaxLength(50);

            builder.Property(v => v.Seguradora)
                .HasMaxLength(100);

            // Configurações de documentação e anexos
            builder.Property(v => v.Documentacao)
                .HasMaxLength(1000); // JSON ou URLs

            builder.Property(v => v.FotosAnexos)
                .HasMaxLength(2000); // JSON ou URLs

            builder.Property(v => v.Observacoes)
                .HasMaxLength(1000);

            // Indexes
            builder.HasIndex(v => v.Placa)
                .IsUnique();

            builder.HasIndex(v => v.Renavam)
                .IsUnique();

            builder.HasIndex(v => v.Chassi)
                .IsUnique();

            builder.HasIndex(v => v.LocadoraId);

            builder.HasIndex(v => new { v.Marca, v.Modelo });

            builder.HasIndex(v => v.Status);

            // Relationships
            builder.HasOne(v => v.Locadora)
                .WithMany(l => l.Veiculos)
                .HasForeignKey(v => v.LocadoraId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(v => v.Locacoes)
                .WithOne(l => l.Veiculo)
                .HasForeignKey(l => l.VeiculoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}