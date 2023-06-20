using ItemBookingApp_API.Domain.Models.Identity;
using ItemBookingApp_API.Domain.Models;

namespace ItemBookingApp_API.Resources.Basket
{
    public class SaveCustomerBasketResource
    {
        public List<BasketItemResource> Items { get; set; } = new List<BasketItemResource>();

        public long UserId { get; set; }
    }
}
