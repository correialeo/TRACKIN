using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Trackin.Domain.Enums;
using Trackin.Domain.Entity;

namespace Trackin.Infrastructure.Mappings
{
    public class MotoMapping : IEntityTypeConfiguration<Moto>
    {
        public void Configure(EntityTypeBuilder<Moto> builder)
        {
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Id).ValueGeneratedOnAdd();

            builder.Property(m => m.Placa).IsRequired().HasMaxLength(10);
            builder.Property(m => m.Modelo).IsRequired().HasMaxLength(50);

            builder.Property(m => m.Modelo).HasConversion(
                v => v.ToString(), 
                v => (ModeloMoto)Enum.Parse(typeof(ModeloMoto), v) 
            );

            builder.Property(m => m.Ano).IsRequired();

            builder.Property(m => m.Status).IsRequired().HasMaxLength(20).HasConversion(
                v => v.ToString(),
                v => (MotoStatus)Enum.Parse(typeof(MotoStatus), v)
            );

            builder.Property(m => m.RFIDTag).IsRequired().HasMaxLength(50);
            builder.Property(m => m.UltimaManutencao).IsRequired(false); 
            builder.Property(m => m.ImagemReferencia).HasMaxLength(255).IsRequired(false);
            builder.Property(m => m.CaracteristicasVisuais).HasColumnType("CLOB").IsRequired(false);

            builder.HasOne(m => m.Patio)
                .WithMany()
                .HasForeignKey(m => m.PatioId)
                .OnDelete(DeleteBehavior.Cascade);



        }
    }
}
