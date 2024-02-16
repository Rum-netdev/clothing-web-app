namespace ClothStoreApp.Handler.Products.Dtos
{
    public class ProductDto
    {
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string QuantityPerUnit { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
    }
}
