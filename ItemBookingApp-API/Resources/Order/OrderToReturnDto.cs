using ItemBookingApp_API.Domain.Models.OrderAggregate;

namespace ItemBookingApp_API.Resources.Order
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }
        public string BorrowerEmail { get; set; } = string.Empty;
        public DateTimeOffset OrderDate { get; set; }

        public Address ShipToAddress { get; set; }
        public string DeliveryMethod { get; set; }
        public decimal ShippingPrice { get; set; }

        public IReadOnlyList<OrderItemDto> OrderItems { get; set; }

        public decimal SubTotal { get; set; }
        public string Status { get; set; }

        public decimal Total { get; set; }
    }
}
