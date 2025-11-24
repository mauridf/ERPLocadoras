using Microsoft.EntityFrameworkCore;
using ERPLocadoras.Core.Entities;
using ERPLocadoras.Infra.Data.Configurations;

namespace ERPLocadoras.Infra.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Locadora> Locadoras { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Applying configurations
            modelBuilder.ApplyConfiguration(new LocadoraConfiguration());
            modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
        }
    }
}