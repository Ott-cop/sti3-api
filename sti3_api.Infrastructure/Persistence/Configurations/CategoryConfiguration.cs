using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sti3_api.Domain.Entities;

namespace sti3_api.Infrastructure.Persistence.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Name);

            builder.Property(c => c.Name)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(c => c.PercentDiscount)
                .HasPrecision(3, 2)
                .IsRequired();
            
            builder.Property(c => c.ConditionalDiscount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.HasData([
                new Category {
                    Name = "REGULAR",
                    PercentDiscount = 0.05m,
                    ConditionalDiscount = 500
                },
                new Category {
                    Name = "PREMIUM",
                    PercentDiscount = 0.10m,
                    ConditionalDiscount = 300
                },
                new Category {
                    Name = "VIP",
                    PercentDiscount = 0.15m,
                    ConditionalDiscount = 0
                },
            ]);
        }
    }
}