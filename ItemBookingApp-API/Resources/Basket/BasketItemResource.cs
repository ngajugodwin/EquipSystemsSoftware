using ItemBookingApp_API.Domain.Models;

namespace ItemBookingApp_API.Resources.Basket
{
    public class BasketItemResource
    {
        public int Quantity { get; set; }
        public int ItemId { get; set; }
        public int CustomerBasketId { get; set; }

        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Picture { get; set; } = string.Empty;


    }

}
