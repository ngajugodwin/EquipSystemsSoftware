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

namespace ItemBookingApp_API.Areas.SuperAdmin
{
    [Route("super-admin/api/[controller]")]
    [ApiController]
    [Authorize(Policy = PermissionSystemName.AccessSuperAdminArea)]
    public class ReportOrdersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IReportOrderService _reportOrderService;

        public ReportOrdersController(IMapper mapper, IReportOrderService reportOrderService)
        {
            _mapper = mapper;
            _reportOrderService = reportOrderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetReportBookings([FromQuery] OrderReportQuery orderReportQuery)
        {

            if (orderReportQuery.StartDate.HasValue && orderReportQuery.EndDate.HasValue)
            {
                if (orderReportQuery.EndDate < orderReportQuery.StartDate)
                    return BadRequest("End date cannot be less than start date");
            }

            var orderReport = await _reportOrderService.GetOrderReport(orderReportQuery);

            var ordersToReturn = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orderReport);

            Response.AddPagination(orderReport.CurrentPage, orderReport.PageSize, orderReport.TotalCount, orderReport.TotalPages);

            return Ok(ordersToReturn);
        }

        [HttpGet("ExportOrders")]
        public async Task<IActionResult> ExportOrders([FromQuery] OrderReportQuery orderReportQuery)
        {
            var result = await _reportOrderService.ExportOrders(orderReportQuery);

            if (result == null)
                return BadRequest("Unable to export data");

            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }


    }
}
