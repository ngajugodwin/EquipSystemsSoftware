using ItemBookingApp_API.Domain.Models.Identity;
using ItemBookingApp_API.Domain.Models;

namespace ItemBookingApp_API.Resources.Basket
{
    public class  IBasketItem
    {
        public int Quantity { get; set; }
        public int ItemId { get; set; }
    }
    public class SaveCustomerBasketResource
    {
        //  public int Id { get; set; }
        //  public List<BasketItemResource> Items { get; set; } = new List<BasketItemResource>();

        public IEnumerable<IBasketItem> Items { get; set; }

        public long UserId { get; set; }

}
}
