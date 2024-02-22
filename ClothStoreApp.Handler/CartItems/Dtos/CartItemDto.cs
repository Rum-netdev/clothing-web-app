namespace ClothStoreApp.Handler.CartItems.Dtos
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string ProductName { get; set; }
    }
}
