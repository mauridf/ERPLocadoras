using Microsoft.EntityFrameworkCore;
using ERPLocadoras.Core.Entities;

namespace ERPLocadoras.Infra.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets serão adicionados nos próximos passos
        public DbSet<Locadora> Locadoras { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações das entidades serão adicionadas nos próximos passos
        }
    }
}