namespace ItemBookingApp_API.Domain.Models
{
    public class BasketItem
    {
        public int Quantity { get; set; }
        public int ItemId { get; set; }
        public virtual Item Item { get; set; }
        public int CustomerBasketId { get; set; }
        public virtual CustomerBasket CustomerBasket { get; set; }


    }
}
