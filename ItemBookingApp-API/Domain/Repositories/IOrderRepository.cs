using ItemBookingApp_API.Domain.Models.OrderAggregate;

namespace ItemBookingApp_API.Domain.Repositories
{
    public interface IOrderRepository
    {
        Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string borrowerEmail);

        Task<Order> GetOrderByIdAsync(int id, string borrowerEmail);
    }
}
