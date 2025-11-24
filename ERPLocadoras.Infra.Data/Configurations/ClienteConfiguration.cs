using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERPLocadoras.Core.Entities;

namespace ERPLocadoras.Infra.Data.Configurations
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Clientes");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.NomeCompleto)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.NomeSocial)
                .HasMaxLength(200);

            builder.Property(c => c.Sexo)
                .HasMaxLength(20);

            builder.Property(c => c.Telefone)
                .HasMaxLength(20);

            builder.Property(c => c.FotoUrl)
                .HasMaxLength(500);

            // Dados de Endereço
            builder.Property(c => c.CEP)
                .HasMaxLength(10);

            builder.Property(c => c.Logradouro)
                .HasMaxLength(200);

            builder.Property(c => c.Numero)
                .HasMaxLength(10);

            builder.Property(c => c.Complemento)
                .HasMaxLength(100);

            builder.Property(c => c.Bairro)
                .HasMaxLength(100);

            builder.Property(c => c.Cidade)
                .HasMaxLength(100);

            builder.Property(c => c.UF)
                .HasMaxLength(2);

            builder.Property(c => c.Pais)
                .HasMaxLength(50)
                .HasDefaultValue("Brasil");

            // Indexes
            builder.HasIndex(c => c.UsuarioId);
        }
    }
}