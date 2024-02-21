using ClothStoreApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothStoreApp.Data.Configurations
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.ToTable($"{nameof(OrderDetail)}s");
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Quantity).IsRequired();
            builder.Property(t => t.UnitPrice).IsRequired();
            builder.Property(t => t.DiscountPercent).IsRequired();

            builder.HasOne(t => t.Order)
                .WithMany(d => d.OrderDetails)
                .HasForeignKey(f => f.OrderId);

            builder.HasOne(t => t.Product)
                .WithMany(d => d.OrderDetail)
                .HasForeignKey(f => f.ProductId);
        }
    }
}
