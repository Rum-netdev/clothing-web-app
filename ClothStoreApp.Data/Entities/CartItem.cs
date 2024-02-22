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
        public decimal UnitPrice { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
