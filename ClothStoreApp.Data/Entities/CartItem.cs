using ClothStoreApp.Data.Entities.Shared;

namespace ClothStoreApp.Data.Entities
{
    public class CartItem : IEntityId, IAuditableEntity
    {
        public int Id { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }

        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
        public int CartId { get; set; }
        public Cart Cart { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
