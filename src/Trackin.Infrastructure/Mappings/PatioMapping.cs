using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Trackin.Domain.Entity;

namespace Trackin.Infrastructure.Mappings
{
    public class PatioMapping : IEntityTypeConfiguration<Patio>
    {
        public void Configure(EntityTypeBuilder<Patio> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.Nome).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Endereco).IsRequired().HasMaxLength(255);
            builder.Property(p => p.Cidade).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Estado).IsRequired().HasMaxLength(50);
            builder.Property(p => p.Pais).IsRequired().HasMaxLength(50);
            builder.Property(p => p.DimensaoX).IsRequired();
            builder.Property(p => p.DimensaoY).IsRequired();
            builder.Property(p => p.PlantaBaixa).HasMaxLength(255);

            builder.HasMany(p => p.Zonas) 
                   .WithOne(z => z.Patio)
                   .HasForeignKey(z => z.PatioId);
        }
    }
}
