using ItemBookingApp_API.Domain.Models.OrderAggregate;

namespace ItemBookingApp_API.Resources.Order
{
    public class OrderDto
    {
        public int BasketId { get; set; }
        public int DeliveryMethodId { get; set; }
        public AddressDto ShipToAddress { get; set; }
        public string? PaymentIntentId { get; set; }
        public BookingInfoDto BookingInfoDto { get; set; }

    }
}
