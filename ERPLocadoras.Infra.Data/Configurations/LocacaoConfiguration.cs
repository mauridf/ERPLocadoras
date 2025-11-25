using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERPLocadoras.Core.Entities;
using ERPLocadoras.Core.Enums;

namespace ERPLocadoras.Infra.Data.Configurations
{
    public class LocacaoConfiguration : IEntityTypeConfiguration<Locacao>
    {
        public void Configure(EntityTypeBuilder<Locacao> builder)
        {
            builder.ToTable("Locacoes");

            builder.HasKey(l => l.Id);

            // Configurações de datas
            builder.Property(l => l.DataInicio)
                .IsRequired();

            builder.Property(l => l.DataPrevistaDevolucao)
                .IsRequired();

            builder.Property(l => l.DataRealDevolucao)
                .IsRequired(false);

            // Configurações de tipo e situação
            builder.Property(l => l.TipoLocacao)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(l => l.Situacao)
                .IsRequired()
                .HasConversion<int>();

            // Configurações de valores
            builder.Property(l => l.ValorDiaria)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(l => l.ValorKmAdicional)
                .HasColumnType("decimal(10,2)");

            builder.Property(l => l.FranquiaKmInclusa)
                .HasColumnType("decimal(10,2)");

            builder.Property(l => l.ValorCaucao)
                .HasColumnType("decimal(10,2)");

            builder.Property(l => l.ValorTotalPrevisto)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(l => l.ValorTotalFinal)
                .HasColumnType("decimal(10,2)");

            builder.Property(l => l.DescontosAcrescimos)
                .HasColumnType("decimal(10,2)");

            // Configurações de formas de pagamento
            builder.Property(l => l.FormaCobranca)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(l => l.FormaCaucao)
                .IsRequired()
                .HasConversion<int>();

            // Configurações de quilometragem
            builder.Property(l => l.KmEntrega)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(l => l.KmDevolucao)
                .HasColumnType("decimal(10,2)");

            // Configurações de texto
            builder.Property(l => l.PlanoLocacao)
                .HasMaxLength(100);

            builder.Property(l => l.ResponsavelEntrega)
                .HasMaxLength(100);

            builder.Property(l => l.ResponsavelDevolucao)
                .HasMaxLength(100);

            builder.Property(l => l.NivelCombustivelEntrega)
                .HasMaxLength(20);

            builder.Property(l => l.NivelCombustivelDevolucao)
                .HasMaxLength(20);

            builder.Property(l => l.ObservacoesInternas)
                .HasMaxLength(1000);

            // Configurações de JSON
            builder.Property(l => l.ChecklistEntrega)
                .HasMaxLength(2000); // JSON

            builder.Property(l => l.ChecklistDevolucao)
                .HasMaxLength(2000); // JSON

            builder.Property(l => l.Anexos)
                .HasMaxLength(3000); // JSON

            // Indexes
            builder.HasIndex(l => l.LocadoraId);
            builder.HasIndex(l => l.VeiculoId);
            builder.HasIndex(l => l.ClienteId);
            builder.HasIndex(l => l.Situacao);
            builder.HasIndex(l => l.DataInicio);
            builder.HasIndex(l => l.DataPrevistaDevolucao);

            // Relationships
            builder.HasOne(l => l.Locadora)
                .WithMany(l => l.Locacoes)
                .HasForeignKey(l => l.LocadoraId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(l => l.Veiculo)
                .WithMany(v => v.Locacoes)
                .HasForeignKey(l => l.VeiculoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(l => l.Cliente)
                .WithMany(c => c.Locacoes)
                .HasForeignKey(l => l.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}