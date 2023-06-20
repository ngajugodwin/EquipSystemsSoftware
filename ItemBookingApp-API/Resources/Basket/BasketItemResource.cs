using ItemBookingApp_API.Domain.Models;

namespace ItemBookingApp_API.Resources.Basket
{
    public class BasketItemResource
    {
        public int Quantity { get; set; }
        public int ItemId { get; set; }
        public int CustomerBasketId { get; set; }

        public string Item { get; set; } = string.Empty;

    }

}
