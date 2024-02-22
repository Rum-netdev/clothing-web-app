using ClothStoreApp.Data.Entities;
using ClothStoreApp.Data.Entities.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothStoreApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<
        ApplicationUser,
        ApplicationRole,
        int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            #region Identity configurations
            builder.Entity<IdentityUserRole<int>>(builder =>
            {
                builder.ToTable("UserRoles");
                builder.HasKey(t => new { t.UserId, t.RoleId });
            });

            builder.Entity<IdentityRoleClaim<int>>(builder =>
            {
                builder.ToTable("RoleClaims");
                builder.HasKey(t => t.Id);
            });

            builder.Entity<IdentityRoleClaim<int>>(builder =>
            {
                builder.ToTable("RoleClaims");
                builder.HasKey(t => t.Id);
            });

            builder.Entity<IdentityUserClaim<int>>(builder =>
            {
                builder.ToTable("UserClaims");
                builder.HasKey(t => t.Id);
            });

            builder.Entity<IdentityUserLogin<int>>(builder =>
            {
                builder.ToTable("UserLogins");
                builder.HasKey(t => t.UserId);
            });

            builder.Entity<IdentityUserToken<int>>(builder =>
            {
                builder.ToTable("UserTokens");
                builder.HasKey(t => t.UserId);
            });
            #endregion
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            foreach(var entry in ChangeTracker.Entries<IAuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt= DateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.ModifiedAt = DateTime.Now;
                        break;
                }
            }
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<ApplicationRole> Roles { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
    }
}
