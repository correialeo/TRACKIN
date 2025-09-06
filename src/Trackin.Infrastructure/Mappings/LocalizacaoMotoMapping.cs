using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Trackin.Domain.Entity;

namespace Trackin.Infrastructure.Mappings
{
    public class LocalizacaoMotoMapping : IEntityTypeConfiguration<LocalizacaoMoto>
    {
        public void Configure(EntityTypeBuilder<LocalizacaoMoto> builder)
        {
            builder.HasKey(l => l.Id);
            builder.Property(l => l.Id).ValueGeneratedOnAdd();

            builder.Property(l => l.CoordenadaX).IsRequired();
            builder.Property(l => l.CoordenadaY).IsRequired();
            builder.Property(l => l.Timestamp).IsRequired();
            builder.Property(l => l.Status).HasConversion<string>() 
                   .IsRequired()
                   .HasMaxLength(20);
            builder.Property(l => l.FonteDados).HasConversion<string>() 
                   .IsRequired()
                   .HasMaxLength(20);
            builder.Property(l => l.Confiabilidade).IsRequired();

            builder.HasOne(l => l.Moto)
                   .WithMany(m => m.Localizacoes)
                   .HasForeignKey(l => l.MotoId);

            builder.HasOne(l => l.Patio) 
                   .WithMany(p => p.Localizacoes)
                   .HasForeignKey(l => l.PatioId);
        }
    }
}
