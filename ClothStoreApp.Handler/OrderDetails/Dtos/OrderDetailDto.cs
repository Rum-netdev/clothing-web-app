namespace ClothStoreApp.Handler.OrderDetails.Dtos
{
    public class OrderDetailDto
    {
        public int Quantity { get; set; }
        public double DiscountPercent { get; set; }
        public decimal UnitPrice { get; set; }
        public int ProductId { get; set; }
    }
}
