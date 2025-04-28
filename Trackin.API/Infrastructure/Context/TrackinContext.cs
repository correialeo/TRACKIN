using Microsoft.EntityFrameworkCore;
using Trackin.API.Domain.Entity;

namespace Trackin.API.Infrastructure.Context
{
    public class TrackinContext(DbContextOptions<TrackinContext> options) : DbContext(options)
    {
        public DbSet<LocalizacaoMoto> Localizacoes { get; set; }
        public DbSet<Moto> Motos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Patio> Patios { get; set; }
        public DbSet<Filial> Filiais { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TrackinContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
