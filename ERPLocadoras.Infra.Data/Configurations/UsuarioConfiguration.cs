using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERPLocadoras.Core.Entities;

namespace ERPLocadoras.Infra.Data.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.SenhaHash)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.Tipo)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(u => u.Permissoes)
                .HasMaxLength(1000);

            builder.Property(u => u.Ativo)
                .IsRequired();

            // Indexes
            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.HasIndex(u => u.LocadoraId);

            // Relationships
            builder.HasOne(u => u.Locadora)
                .WithMany(l => l.Usuarios)
                .HasForeignKey(u => u.LocadoraId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(u => u.Pessoa)
                .WithOne(p => p.Usuario)
                .HasForeignKey<Pessoa>(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}