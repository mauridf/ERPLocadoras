using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERPLocadoras.Core.Entities;

namespace ERPLocadoras.Infra.Data.Configurations
{
    public class PessoaConfiguration : IEntityTypeConfiguration<Pessoa>
    {
        public void Configure(EntityTypeBuilder<Pessoa> builder)
        {
            builder.ToTable("Pessoas");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.NomeCompleto)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.NomeSocial)
                .HasMaxLength(200);

            builder.Property(p => p.Sexo)
                .HasMaxLength(20);

            builder.Property(p => p.Telefone)
                .HasMaxLength(20);

            builder.Property(p => p.FotoUrl)
                .HasMaxLength(500);

            // Dados de Endereço
            builder.Property(p => p.CEP)
                .HasMaxLength(10);

            builder.Property(p => p.Logradouro)
                .HasMaxLength(200);

            builder.Property(p => p.Numero)
                .HasMaxLength(10);

            builder.Property(p => p.Complemento)
                .HasMaxLength(100);

            builder.Property(p => p.Bairro)
                .HasMaxLength(100);

            builder.Property(p => p.Cidade)
                .HasMaxLength(100);

            builder.Property(p => p.UF)
                .HasMaxLength(2);

            builder.Property(p => p.Pais)
                .HasMaxLength(50)
                .HasDefaultValue("Brasil");

            // Indexes
            builder.HasIndex(p => p.UsuarioId)
                .IsUnique();
        }
    }
}