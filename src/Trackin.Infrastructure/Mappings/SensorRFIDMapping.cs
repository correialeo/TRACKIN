using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trackin.Domain.Entity;

namespace Trackin.Infrastructure.Mappings
{
    public class SensorRFIDMapping : IEntityTypeConfiguration<SensorRFID>
    {
        public void Configure(EntityTypeBuilder<SensorRFID> builder)
        {
            builder.ToTable("SensorRFID");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Posicao).IsRequired().HasMaxLength(100);
            builder.Property(s => s.Altura).IsRequired();
            builder.Property(s => s.AnguloVisao).IsRequired();

            builder.HasOne(s => s.Patio)
                .WithMany(p => p.SensoresRFID)
                .HasForeignKey(s => s.PatioId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.ZonaPatio)
                .WithMany(z => z.SensoresRFID)
                .HasForeignKey(s => s.ZonaPatioId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.OwnsOne(c => c.PosicaoSensor, coordenada =>
            {
                coordenada.Property(p => p.X)
                          .HasColumnName("PosicaoX")
                          .IsRequired();

                coordenada.Property(p => p.Y)
                          .HasColumnName("PosicaoY")
                          .IsRequired();
            });
        }
    } 
}
