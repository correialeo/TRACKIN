using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Trackin.API.Domain.Entity;

namespace Trackin.API.Infrastructure.Mappings
{
    public class CameraMapping : IEntityTypeConfiguration<Camera>
    {
        public void Configure(EntityTypeBuilder<Camera> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            builder.Property(c => c.Posicao).IsRequired().HasMaxLength(50);
            builder.Property(c => c.PosicaoX).IsRequired();
            builder.Property(c => c.PosicaoY).IsRequired();
            builder.Property(c => c.Altura).IsRequired();
            builder.Property(c => c.AnguloVisao).IsRequired();
            builder.Property(c => c.Status).HasConversion<string>() 
                   .IsRequired()
                   .HasMaxLength(20);
            builder.Property(c => c.URL).HasMaxLength(255);

            builder.HasOne(c => c.Patio) 
                   .WithMany(p => p.Camera)
                   .HasForeignKey(c => c.PatioId);
        }
    }
}
