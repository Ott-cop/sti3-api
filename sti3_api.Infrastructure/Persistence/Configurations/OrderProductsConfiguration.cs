using Microsoft.EntityFrameworkCore;
using sti3_api.Domain.Entities;

namespace sti3_api.Infrastructure.Persistence.Configurations
{
    public class OrderProductsConfiguration : IEntityTypeConfiguration<OrderProduct>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<OrderProduct> builder)
        {
            builder.HasKey(op => op.OrderProductId);

            builder.HasOne(op => op.Order)
                .WithMany(o => o.OrderProducts)
                .HasForeignKey(op => op.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(op => op.Product)
                .WithMany()
                .HasForeignKey(op => op.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(op => op.Quantity).IsRequired();
            builder.Property(op => op.UnitPrice).IsRequired();
        }
    }
}