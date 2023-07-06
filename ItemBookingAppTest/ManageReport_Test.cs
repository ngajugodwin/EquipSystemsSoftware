using AutoMapper;
using ItemBookingApp_API.Areas.SuperAdmin;
using ItemBookingApp_API.Areas.SuperAdmin.Controllers;
using ItemBookingApp_API.Domain.Models.OrderAggregate;
using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Domain.Services;
using ItemBookingApp_API.Domain.Services.Communication;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemBookingAppTest
{
   

    public class ManageReport_Test
    {
        public Mock<IReportOrderService> mock = new Mock<IReportOrderService>();

        public Mock<IMapper> mapper = new Mock<IMapper>();


        [Fact]
        public async void ExportReport_Test()
        {
            var query = new OrderReportQuery
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                PageNumber = 1,
                PageSize = 10
            };

            var memory = new MemoryStream { Capacity = 2, Position = 0 };

            mock.Setup(p => p.ExportOrders(query)).ReturnsAsync(memory);

            ReportOrdersController reportController = new ReportOrdersController(mapper.Object, mock.Object);

            var result = await reportController.ExportOrders(query);

            Assert.NotNull(result);
        }

    }
}
