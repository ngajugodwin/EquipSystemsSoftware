using ItemBookingApp_API.Domain.Models.OrderAggregate;
using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Resources.Query;

namespace ItemBookingApp_API.Domain.Services
{
    public interface IReportOrderService
    {
        Task<PagedList<Order>> GetOrderReport(OrderReportQuery orderReportQuery);
        Task<MemoryStream> ExportOrders(OrderReportQuery orderReportQuery);
    }
}
