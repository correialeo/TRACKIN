using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Trackin.Domain.Entity;

namespace Trackin.Infrastructure.Mappings
{
    public class CameraMapping : IEntityTypeConfiguration<Camera>
    {
        public void Configure(EntityTypeBuilder<Camera> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            builder.Property(c => c.Posicao).IsRequired().HasMaxLength(50);
            builder.Property(c => c.Altura).IsRequired();
            builder.Property(c => c.AnguloVisao).IsRequired();
            builder.Property(c => c.Status).HasConversion<string>() 
                   .IsRequired()
                   .HasMaxLength(20);
            builder.Property(c => c.URL).HasMaxLength(255);

            builder.HasOne(c => c.Patio) 
                   .WithMany(p => p.Cameras)
                   .HasForeignKey(c => c.PatioId);

            builder.OwnsOne(c => c.PosicaoPatio, coordenada =>
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
