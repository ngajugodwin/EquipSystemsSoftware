using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Domain.Models.OrderAggregate;

namespace ItemBookingApp_API.Domain.Services.Communication
{
    public class OrderResponse : BaseResponse<Order>
    {
        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="order">Order response.</param>
        /// <returns>Response.</returns>
        public OrderResponse(Order order) : base(order)
        { }

        /// <summary>
        /// Creates an error response.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public OrderResponse(string message) : base(message)
        { }
    }
}
