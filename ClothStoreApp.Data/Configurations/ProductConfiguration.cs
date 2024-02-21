using ClothStoreApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothStoreApp.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(t => t.Id);

            builder.Property(t => t.ProductName).IsRequired().HasMaxLength(255);
            builder.Property(t => t.QuantityPerUnit).IsRequired().HasMaxLength(20);
            builder.Property(t => t.UnitsInStock).IsRequired().HasDefaultValue(0);
            builder.Property(t => t.UnitsOnOrder).IsRequired().HasDefaultValue(0);
            builder.Property(t => t.ReorderLevel).IsRequired().HasDefaultValue(0);

            builder.HasOne(t => t.Category)
                .WithMany(t => t.Products)
                .HasForeignKey(f => f.CategoryId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
