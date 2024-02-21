using ClothStoreApp.Data.Entities.Shared;

namespace ClothStoreApp.Data.Entities
{
    public class Order : IEntityId
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime DeliveredDate { get; set; }

        // Receiver information
        public string ReceiverName { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverCity { get; set; }
        public string ReceiverCountry { get; set; }
        public string ReceiverPostalCode { get; set; }

        // Shipper or delivery man information
        public int DeliveryVia { get; set; }
        public decimal Freight { get; set; }

        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    }
}
