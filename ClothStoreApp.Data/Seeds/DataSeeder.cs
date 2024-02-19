using ClothStoreApp.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace ClothStoreApp.Data.Seeds
{
    public static class DataSeeder
    {
        public static void Seed(ApplicationDbContext db)
        {
            SeedRoles(db);
            SeedUsers(db);
        }

        private static void SeedUsers(ApplicationDbContext db)
        {
            if(!db.Users.Any())
            {
                PasswordHasher<ApplicationUser> hasher = new PasswordHasher<ApplicationUser>();
                ApplicationUser user = new ApplicationUser()
                {
                    UserName = "jinkey.coredev",
                    NormalizedUserName = "jinkey.coredev".ToUpper(),
                    Email = "jinkey.coredev@gmail.com",
                    NormalizedEmail = "jinkey.coredev@gmail.com".ToUpper(),
                    EmailConfirmed = true,
                    PhoneNumber = "0795671811",
                    PhoneNumberConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "Admin123"),
                    FirstName = "Quang",
                    LastName = "Ngo Viet",
                    Dob = new DateTime(2001, 11, 18),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                };
                db.Users.Add(user);
                db.SaveChanges();

                ApplicationRole role = db.Roles.Where(t => t.Name == "Admin").FirstOrDefault();
                db.UserRoles.Add(new IdentityUserRole<int>() { UserId = user.Id, RoleId = role.Id });
                db.SaveChanges();
            }
        }

        private static void SeedRoles(ApplicationDbContext db)
        {
            string[] basicRoles = new[] { "Admin", "Moderator", "Basic" };
            var roles = new List<ApplicationRole>();

            if (!db.Roles.Any())
            {
                foreach (var role in basicRoles)
                {
                    roles.Add(new ApplicationRole()
                    {
                        Name = role,
                        NormalizedName = role.ToUpper(),
                        ConcurrencyStamp = Guid.NewGuid().ToString()
                    });
                }

                db.Roles.AddRange(roles);
                db.SaveChanges();
            }
        }
    }
}
