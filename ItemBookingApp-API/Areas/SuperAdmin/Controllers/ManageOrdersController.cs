using AutoMapper;
using ItemBookingApp_API.Domain.Models.OrderAggregate;
using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Domain.Services;
using ItemBookingApp_API.Extension;
using ItemBookingApp_API.Resources.Order;
using ItemBookingApp_API.Services;
using ItemBookingApp_API.Services.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ItemBookingApp_API.Areas.SuperAdmin.Controllers
{
    //[Route("api/[controller]")]
    [Route("super-admin/api/[controller]")]
    [ApiController]
    [Authorize(Policy = PermissionSystemName.AccessSuperAdminArea)]
    public class ManageOrdersController : ControllerBase
    {
        private readonly IManageOrderService _manageOrderService;
        private readonly IMapper _mapper;

        public ManageOrdersController(IManageOrderService manageOrderService, IMapper mapper)
        {
            _manageOrderService = manageOrderService;
            _mapper = mapper;
        }

        [HttpGet("getOrderDetails/{orderId}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int orderId)
        {
            var order = await _manageOrderService.GetOrderByIdAsync(orderId);

            if (order == null)
                return NotFound("Not found");

            return _mapper.Map<Order, OrderToReturnDto>(order.Resource);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrdersForModeration([FromQuery] OrderQuery orderQuery)
        {
            var ordersForModeration = await _manageOrderService.GetBookingsListForModerationAsync(orderQuery);

            var ordersToReturn = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(ordersForModeration);

            Response.AddPagination(ordersForModeration.CurrentPage, ordersForModeration.PageSize, ordersForModeration.TotalCount, ordersForModeration.TotalPages);

            return Ok(ordersToReturn);
        }

        [HttpPost("{orderId}")]
        public async Task<IActionResult> ApproveBooking(int orderId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var email = HttpContext.User.RetrieveEmailFromPrincipal();

            var result = await _manageOrderService.ApproveOrder(email, orderId);           

            if (!result.Success)
                return BadRequest(result.Message);

            var approvedBookingToReturn = _mapper.Map<Order, OrderToReturnDto>(result.Resource);

            return Ok(approvedBookingToReturn);
        }

        [HttpPost("closeOrder/{orderId}")]
        public async Task<IActionResult> CloseBooking(int orderId)
        {
            var result = await _manageOrderService.CloseOrder(orderId);

            if (!result.Success)
                return BadRequest(result.Message);

            var rejectedBookingToReturn = _mapper.Map<Order, OrderToReturnDto>(result.Resource);

            return Ok(rejectedBookingToReturn);
        }

        [HttpDelete("{orderId}")]
        public async Task<IActionResult> RejectBooking(int orderId)
        {
            var result = await _manageOrderService.RejectOrder(orderId);

            if (!result.Success)
                return BadRequest(result.Message);

            var rejectedBookingToReturn = _mapper.Map<Order, OrderToReturnDto>(result.Resource);

            return Ok(rejectedBookingToReturn);
        }


    }
}
