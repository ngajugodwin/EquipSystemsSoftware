using ItemBookingApp_API.Domain.Models.Identity;

namespace ItemBookingApp_API.Domain.Models
{
    public class CustomerBasket
    {
        public int Id { get; set; }
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();

        public long UserId { get; set; }
        public AppUser User { get; set; }
        public int? DeliveryMethodId { get; set; }
        public string? ClientSecret { get; set; }
        public string? PaymentIntentId { get; set; }
        public decimal? ShippingPrice { get; set; }


    }
}
