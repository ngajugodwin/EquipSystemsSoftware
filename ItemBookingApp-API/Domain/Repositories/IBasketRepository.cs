using ItemBookingApp_API.Domain.Models;

namespace ItemBookingApp_API.Domain.Repositories
{
    public interface IBasketRepository
    {
        Task<CustomerBasket> UpdateDeliveryMethod(int basketId, int deliveryMethodId);
        Task<CustomerBasket> GetBasketAsync(long userId, int basketId);
        Task<CustomerBasket> AddBasketAsync(CustomerBasket basket);

        Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket);

        Task<bool> DeleteBasketAsync(long userId, int basketId);

        Task DeleteBasket(int basketId);

        Task<CustomerBasket> AddOneItemToExistingBasket(long userId, int basketId, BasketItem basketItem);

        Task<CustomerBasket> DeleteOneItemFromBasket(long userId, int basketId, int itemId);
        Task<CustomerBasket> IncreaseItemQuantity(long userId, int basketId, int itemId);
        Task<CustomerBasket> DecreaseItemQuantity(long userId, int basketId, int itemId);

        Task<CustomerBasket> GetBasketAsync(int basketId);

    }
}
