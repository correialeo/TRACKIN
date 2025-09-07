using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Trackin.Domain.Entity;

namespace Trackin.Infrastructure.Mappings
{
    public class DeteccaoVisualMapping : IEntityTypeConfiguration<DeteccaoVisual>
    {
        public void Configure(EntityTypeBuilder<DeteccaoVisual> builder)
        {
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Id).ValueGeneratedOnAdd();
            builder.Property(d => d.Confianca).IsRequired();
            builder.Property(d => d.ImagemCaptura).HasMaxLength(255);

            builder.HasOne(d => d.Moto) 
                   .WithMany(m => m.DeteccoesVisuais)
                   .HasForeignKey(d => d.MotoId)
                   .IsRequired(false);

            builder.HasOne(d => d.Camera) 
                   .WithMany(c => c.DeteccoesVisuais)
                   .HasForeignKey(d => d.CameraId);

            builder.OwnsOne(c => c.PosicaoPatio, coordenada =>
            {
                coordenada.Property(p => p.X)
                          .HasColumnName("CoordenadaXPatio")
                          .IsRequired();

                coordenada.Property(p => p.Y)
                          .HasColumnName("CoordenadaYPatio")
                          .IsRequired();
            });

            builder.OwnsOne(c => c.PosicaoImagem, coordenada =>
            {
                coordenada.Property(p => p.X)
                          .HasColumnName("CoordenadaXImagem")
                          .IsRequired();

                coordenada.Property(p => p.Y)
                          .HasColumnName("CoordenadaYImagem")
                          .IsRequired();
            });
        }
    }
}
