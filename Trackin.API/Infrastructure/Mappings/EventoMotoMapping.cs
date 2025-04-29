using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Trackin.API.Domain.Entity;

namespace Trackin.API.Infrastructure.Mappings
{
    public class EventoMotoMapping : IEntityTypeConfiguration<EventoMoto>
    {
        public void Configure(EntityTypeBuilder<EventoMoto> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.Tipo).HasConversion<string>()
                   .IsRequired()
                   .HasMaxLength(20);
            builder.Property(e => e.Timestamp).IsRequired();
            builder.Property(e => e.FonteEvento).HasConversion<string>()
                   .IsRequired()
                   .HasMaxLength(20);
            builder.Property(e => e.Observacao).HasMaxLength(500);

            builder.HasOne(e => e.Moto) 
                   .WithMany(m => m.Eventos)
                   .HasForeignKey(e => e.MotoId);

            //config pra relacao com usuario, camera e sensor
            builder.HasOne(e => e.Usuario)
                     .WithMany(u => u.Eventos)
                     .HasForeignKey(e => e.UsuarioId)
                     .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Camera)
                        .WithMany(c => c.Eventos)
                        .HasForeignKey(e => e.CameraId)
                        .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Sensor)
                        .WithMany(s => s.Eventos)
                        .HasForeignKey(e => e.SensorId)
                        .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
