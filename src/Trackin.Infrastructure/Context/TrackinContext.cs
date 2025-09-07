using Microsoft.EntityFrameworkCore;
using Trackin.Domain.Entity;

namespace Trackin.Infrastructure.Context
{
    public class TrackinContext(DbContextOptions<TrackinContext> options) : DbContext(options)
    {
        public DbSet<LocalizacaoMoto> Localizacoes { get; set; }
        public DbSet<Moto> Motos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Patio> Patios { get; set; }
        public DbSet<Camera> Cameras { get; set; }
        public DbSet<SensorRFID> SensoresRFID { get; set; }
        public DbSet<ZonaPatio> ZonasPatio { get; set; }
        public DbSet<EventoMoto> Eventos { get; set; }
        public DbSet<DeteccaoVisual> DeteccoesVisuais { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TrackinContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
