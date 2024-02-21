using ClothStoreApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothStoreApp.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable($"{nameof(Order)}s");
            builder.HasKey(t => t.Id);

            builder.Property(t => t.ReceiverName).IsRequired();
            builder.Property(t => t.ReceiverAddress).IsRequired();
            builder.Property(t => t.ReceiverCity).IsRequired(false);
            builder.Property(t => t.ReceiverCountry).IsRequired(false);
            builder.Property(t => t.ReceiverPostalCode).IsRequired(false);

            builder.HasOne(t => t.User)
                .WithMany(d => d.Orders)
                .HasForeignKey(f => f.UserId);
        }
    }
}
