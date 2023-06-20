using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Domain.Models.Queries;

namespace ItemBookingApp_API.Resources.Basket
{
    public class CustomerBasketQuery : BaseQuery
    {
        public long UserId { get; set; }

        public int BasketId { get; set; }

        public BasketItem BasketItem { get; set; }
    }
}
