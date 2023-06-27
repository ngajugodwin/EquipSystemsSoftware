using ItemBookingApp_API.Domain.Models.OrderAggregate;
using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Resources.Query;

namespace ItemBookingApp_API.Domain.Repositories
{
    public interface IOrderRepository
    {
        Task<PagedList<Order>> GetOrderReport(OrderReportQuery orderReportQuery);
        Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string borrowerEmail);

        Task<Order> GetOrderByIdAsync(int id, string borrowerEmail);

        Task<PagedList<Order>> GetOrdersListForModerationAsync(OrderQuery orderQuery);

        Task<Order> GetOrderByIdAsync(int orderId);

        void DeleteOrderAsync(Order order);
    }
}
