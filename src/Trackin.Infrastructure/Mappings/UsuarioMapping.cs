using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Trackin.Domain.Entity;

namespace Trackin.Infrastructure.Mappings
{
    public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();

            builder.Property(u => u.Nome).IsRequired().HasMaxLength(100);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(100);
            builder.Property(u => u.Senha).IsRequired().HasMaxLength(255); 
            builder.Property(u => u.Role).HasConversion<string>() 
                   .IsRequired()
                   .HasMaxLength(20);

            builder.HasOne(u => u.Patio) 
                   .WithMany(p => p.Usuarios)
                   .HasForeignKey(u => u.PatioId);
        }
    }
}
