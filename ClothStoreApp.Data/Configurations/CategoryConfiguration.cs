using ClothStoreApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothStoreApp.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name).IsRequired();
            builder.Property(t => t.ParentId).IsRequired();
            builder.Property(t => t.Description).HasMaxLength(255);
        }
    }
}
