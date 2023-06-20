using ItemBookingApp_API.Domain.Models;

namespace ItemBookingApp_API.Domain.Repositories
{
    public interface IBasketRepository
    {
        Task<CustomerBasket> GetBasketAsync(long userId, int basketId);
        Task<CustomerBasket> AddBasketAsync(CustomerBasket basket);

        Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket);

        Task<bool> DeleteBasketAsync(long userId, int basketId);

        Task<CustomerBasket> AddOneItemToExistingBasket(long userId, int basketId, BasketItem basketItem);

        Task<CustomerBasket> DeleteOneItem(long userId, int basketId, int itemId);
        Task<bool> IncreaseItemQuantity(long userId, int basketId, int itemId, int quantity);
        Task<bool> DecreaseItemQuantity(long userId, int basketId, int itemId, int quantity);

    }
}
