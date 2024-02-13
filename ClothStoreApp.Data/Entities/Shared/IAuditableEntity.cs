namespace ClothStoreApp.Data.Entities.Shared
{
    public interface IAuditableEntity
    {
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
