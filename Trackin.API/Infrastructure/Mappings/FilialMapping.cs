using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Trackin.API.Domain.Entity;

namespace Trackin.API.Infrastructure.Mappings
{
    public class FilialMapping : IEntityTypeConfiguration<Filial>
    {
        public void Configure(EntityTypeBuilder<Filial> builder)
        {
            builder.HasKey(f => f.Id);
            builder.Property(f => f.Id).ValueGeneratedOnAdd();

            builder.Property(f => f.Nome).IsRequired().HasMaxLength(100);
            builder.Property(f => f.Telefone).IsRequired().HasMaxLength(20);
            builder.Property(f => f.Email).IsRequired().HasMaxLength(100);

            builder.HasOne(f => f.Patio) 
                   .WithOne(p => p.Filial)
                   .HasForeignKey<Filial>(f => f.PatioId);

        }
    }
}
