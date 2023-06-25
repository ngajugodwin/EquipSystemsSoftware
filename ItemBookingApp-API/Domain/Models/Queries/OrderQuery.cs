using ItemBookingApp_API.Domain.Models.OrderAggregate;

namespace ItemBookingApp_API.Domain.Models.Queries
{
    public class OrderQuery : BaseQuery
    {
        public DateTimeOffset? OrderDate { get; set; }

        public OrderStatus Status { get; set; }

        public ApprovalStatus BookingStatus { get; set; }
    }
}
