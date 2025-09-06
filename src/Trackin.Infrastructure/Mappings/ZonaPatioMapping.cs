using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Trackin.Domain.Entity;

namespace Trackin.Infrastructure.Mappings
{
    public class ZonaPatioMapping : IEntityTypeConfiguration<ZonaPatio>
    {
        public void Configure(EntityTypeBuilder<ZonaPatio> builder)
        {
            builder.HasKey(z => z.Id);
            builder.Property(z => z.Id).ValueGeneratedOnAdd();

            builder.Property(z => z.Nome).IsRequired().HasMaxLength(50);
            builder.Property(z => z.CoordenadaInicialX).IsRequired();
            builder.Property(z => z.CoordenadaInicialY).IsRequired();
            builder.Property(z => z.CoordenadaFinalX).IsRequired();
            builder.Property(z => z.CoordenadaFinalY).IsRequired();
            builder.Property(z => z.Cor).IsRequired().HasMaxLength(20);

            builder.HasOne(z => z.Patio)
                   .WithMany(p => p.Zonas)
                   .HasForeignKey(z => z.PatioId);
        }
    }
}
