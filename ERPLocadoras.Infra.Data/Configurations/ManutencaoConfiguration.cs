using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERPLocadoras.Core.Entities;
using ERPLocadoras.Core.Enums;

namespace ERPLocadoras.Infra.Data.Configurations
{
    public class ManutencaoConfiguration : IEntityTypeConfiguration<Manutencao>
    {
        public void Configure(EntityTypeBuilder<Manutencao> builder)
        {
            builder.ToTable("Manutencoes");

            builder.HasKey(m => m.Id);

            // Configurações básicas
            builder.Property(m => m.Tipo)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(m => m.Descricao)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(m => m.DataEntrada)
                .IsRequired();

            builder.Property(m => m.KmEntrada)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(m => m.KmSaida)
                .HasColumnType("decimal(10,2)");

            builder.Property(m => m.Status)
                .IsRequired()
                .HasConversion<int>();

            // Configurações de oficina
            builder.Property(m => m.OficinaPrestador)
                .HasMaxLength(200);

            builder.Property(m => m.CnpjContatoOficina)
                .HasMaxLength(20);

            // Configurações de custos
            builder.Property(m => m.CustoPecas)
                .HasColumnType("decimal(10,2)");

            builder.Property(m => m.CustoMaoDeObra)
                .HasColumnType("decimal(10,2)");

            builder.Property(m => m.CustoTotal)
                .HasColumnType("decimal(10,2)");

            // Configurações de garantia
            builder.Property(m => m.GarantiaServico)
                .HasMaxLength(100);

            // Configurações de texto
            builder.Property(m => m.Observacoes)
                .HasMaxLength(2000);

            builder.Property(m => m.Anexos)
                .HasMaxLength(3000); // JSON

            // Indexes
            builder.HasIndex(m => m.LocadoraId);
            builder.HasIndex(m => m.VeiculoId);
            builder.HasIndex(m => m.ResponsavelManutencaoId);
            builder.HasIndex(m => m.Status);
            builder.HasIndex(m => m.DataEntrada);
            builder.HasIndex(m => m.DataSaidaPrevista);

            // Relationships
            builder.HasOne(m => m.Locadora)
                .WithMany()
                .HasForeignKey(m => m.LocadoraId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Veiculo)
                .WithMany()
                .HasForeignKey(m => m.VeiculoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.ResponsavelManutencao)
                .WithMany()
                .HasForeignKey(m => m.ResponsavelManutencaoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}