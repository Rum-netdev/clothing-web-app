using ClothStoreApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothStoreApp.Data.Configurations
{
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.ToTable($"{nameof(CartItem)}s");
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Quantity).IsRequired();
            builder.Property(t => t.UnitPrice).IsRequired();

            builder.HasOne(t => t.User)
                .WithMany(d => d.CartItems)
                .HasForeignKey(f => f.UserId);

            builder.HasOne(t => t.Product)
                .WithMany(d => d.CartItems)
                .HasForeignKey(f => f.ProductId);
        }
    }
}
