using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Trackin.API.Domain.Entity;

namespace Trackin.API.Infrastructure.Mappings
{
    public class DeteccaoVisualMapping : IEntityTypeConfiguration<DeteccaoVisual>
    {
        public void Configure(EntityTypeBuilder<DeteccaoVisual> builder)
        {
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Id).ValueGeneratedOnAdd();

            builder.Property(d => d.CoordenadaXImagem).IsRequired();
            builder.Property(d => d.CoordenadaYImagem).IsRequired();
            builder.Property(d => d.CoordenadaXPatio).IsRequired();
            builder.Property(d => d.CoordenadaYPatio).IsRequired();
            builder.Property(d => d.Confianca).IsRequired();
            builder.Property(d => d.ImagemCaptura).HasMaxLength(255);

            builder.HasOne(d => d.Moto) 
                   .WithMany(m => m.DeteccoesVisuais)
                   .HasForeignKey(d => d.MotoId)
                   .IsRequired(false);

            builder.HasOne(d => d.Camera) 
                   .WithMany(c => c.DeteccoesVisuais)
                   .HasForeignKey(d => d.CameraId);
        }
    }
}
