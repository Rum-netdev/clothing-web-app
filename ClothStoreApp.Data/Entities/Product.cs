using ClothStoreApp.Data.Entities.Shared;

namespace ClothStoreApp.Data.Entities
{
    public class Product : IEntityId, IAuditableEntity
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string QuantityPerUnit { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public int UnitsOnOrder { get; set; }
        public int ReorderLevel { get; set; }
        public bool Discontinued { get; set; }
        public int CreatedBy { get;set; }
        public DateTime CreatedAt { get;set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedAt { get;set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<OrderDetail> OrderDetail { get; set; }
    }
}
