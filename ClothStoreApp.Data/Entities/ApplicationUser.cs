using Microsoft.AspNetCore.Identity;
namespace ClothStoreApp.Data.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Dob { get; set; }
        public string Address { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
