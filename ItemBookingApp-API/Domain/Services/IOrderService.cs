using ItemBookingApp_API.Domain.Models.OrderAggregate;

namespace ItemBookingApp_API.Domain.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string borrowerEmail, int deliveryMethod, int basketId, Address shippingAddress, BookingInformation bookingInformation);

        Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string borrowerEmail);

        Task<Order> GetOrderByIdAsync(int id, string borrowerEmail);

        Task<Order> GetOrderByIdAsync(int id);

        Task<IEnumerable<DeliveryMethod>> GetDeliveryMethodsAsync();

    }
}
