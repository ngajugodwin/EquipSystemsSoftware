using ItemBookingApp_API.Domain.Models.OrderAggregate;
using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Domain.Services.Communication;
using ItemBookingApp_API.Resources.Query;

namespace ItemBookingApp_API.Domain.Services
{
    public interface IManageOrderService
    {
        Task<PagedList<Order>> GetBookingsListForModerationAsync(OrderQuery orderQuery);
        Task<OrderResponse> ApproveOrder(string approvedByUserEmail, int orderId);
        Task<OrderResponse> CloseOrder(int orderId);
        Task<OrderResponse> RejectOrder(int orderId);
    }
}
