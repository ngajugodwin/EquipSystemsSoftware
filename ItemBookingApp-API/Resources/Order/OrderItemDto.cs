using ItemBookingApp_API.Domain.Models.OrderAggregate;

namespace ItemBookingApp_API.Resources.Order
{
    public class OrderItemDto
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
