using AutoMapper;
using ItemBookingApp_API.Domain.Models.OrderAggregate;
using ItemBookingApp_API.Domain.Services;
using ItemBookingApp_API.Extension;
using ItemBookingApp_API.Resources.Order;
using ItemBookingApp_API.Services.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ItemBookingApp_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = PermissionSystemName.HasUserRole)]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal();

            var address = _mapper.Map<AddressDto, Domain.Models.OrderAggregate.Address>(orderDto.ShipToAddress);

            var bookingInfo = _mapper.Map<BookingInfoDto, Domain.Models.OrderAggregate.BookingInformation>(orderDto.BookingInfoDto);

            var order = await _orderService.CreateOrderAsync(email, orderDto.DeliveryMethodId, orderDto.BasketId, address, bookingInfo);

            if (order == null) return BadRequest("Problem create your request");

            return Ok(order);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetOrdersForUser()
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal();

            var orders = await _orderService.GetOrdersForUserAsync(email);

            var orderToReturn = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders);

            return Ok(orderToReturn);
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int orderId)
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal();

            var order = await _orderService.GetOrderByIdAsync(orderId, email);

            if (order == null)
                return NotFound("Not found");

            return _mapper.Map<Order, OrderToReturnDto>(order);
        }

        [HttpGet("getDeliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods(int deliveryId)
        {

            var deliveryMethods = await _orderService.GetDeliveryMethodsAsync();

            return Ok(deliveryMethods);
        }
    }
}
