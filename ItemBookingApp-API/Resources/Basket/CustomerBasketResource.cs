using ItemBookingApp_API.Domain.Models.Identity;
using ItemBookingApp_API.Domain.Models;

namespace ItemBookingApp_API.Resources.Basket
{
    public class CustomerBasketResource
    {
        public int Id { get; set; }
        public List<BasketItemResource> Items { get; set; } = new List<BasketItemResource>();

        public long UserId { get; set; }

        public int? DeliveryMethodId { get; set; }
        public string? ClientSecret { get; set; }
        public string? PaymentIntentId { get; set; }

        public decimal? ShippingPrice { get; set; }



        public CustomerBasketResource()
        {

        }

        public CustomerBasketResource(int id, long userId)
        {
            Id = id;
            UserId = userId;
        }
    }
}
