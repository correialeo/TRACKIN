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
            builder.Property(s => s.PosicaoX).IsRequired();
            builder.Property(s => s.PosicaoY).IsRequired();
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
        }
    } 
}
