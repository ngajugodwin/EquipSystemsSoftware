using AutoMapper;
using ItemBookingApp_API.Areas.SuperAdmin.Controllers;
using ItemBookingApp_API.Domain.Models.OrderAggregate;
using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Domain.Services;
using ItemBookingApp_API.Domain.Services.Communication;
using ItemBookingApp_API.Resources.Query;
using ItemBookingApp_API.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using NPOI.SS.Formula.Functions;
using Moq.EntityFrameworkCore;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;

namespace ItemBookingAppTest
{
    
    public class ManageOrderUnitTest
    {
        public Mock<IManageOrderService> manageOrderMock = new Mock<IManageOrderService>();

        public Mock<IMapper> mapper = new Mock<IMapper>();
        
        [Fact]
        public async void GetOneUserOrderTest()
        {
            var order = new Order { Id = 1, BorrowerEmail = "test@example.com", OrderDate = DateTime.Now };

            manageOrderMock.Setup(p => p.GetOrderByIdAsync(1)).ReturnsAsync(new OrderResponse(order));

            ManageOrdersController orderController = new ManageOrdersController(manageOrderMock.Object, mapper.Object);

            var result = await orderController.GetOrderByIdForUser(1);

            Assert.NotNull(result);
        }

        [Fact]
        public void GetAllOrdersAndReturnAllPendingOrdersForModerationTest()
        {           
            var orders = new List<Order>()
            {
                new Order 
                { 
                    Id = 1, 
                    BorrowerEmail = "test1@example.com", 
                    OrderDate = DateTime.Now, 
                    Status = OrderStatus.Pending 
                },
                 new Order 
                 { Id = 2, 
                     BorrowerEmail = "test2@example.com", 
                     OrderDate = DateTime.Now, 
                     Status = OrderStatus.Pending 
                 }
            };

            Assert.NotEmpty(orders);
        }

        [Fact]
        public async void ApproveOnePendingOrderTest()
        {
            var orderItems = new List<OrderItem>
            {
                new OrderItem
                {
                    Id = 1,
                    Price = 10,
                    Quantity = 1,
                    ItemOrdered = new ItemOrdered
                    {
                        ItemId = 1,
                        Name ="overn",
                        PictureUrl = "testimageURL"
                    }
                }
            };
            var oneOrder = new Order { Id = 1, BorrowerEmail = "test@example.com", OrderDate = DateTime.Now,
                Status = OrderStatus.PaymentReceived , OrderItems = orderItems,
                BookingInformation = new BookingInformation 
                { 
                    Status = ApprovalStatus.Approved, 
                    StartDate = DateTime.Now, 
                    EndDate = DateTime.Now.AddDays(2)
                }
            };


            manageOrderMock.Setup(p => p.GetOrderByIdAsync(oneOrder.Id)).ReturnsAsync(new OrderResponse(oneOrder));

            manageOrderMock.Setup(p => p.ApproveOrder("test@example.com", 1)).ReturnsAsync(new OrderResponse(oneOrder));

            ManageOrdersController orderController = new ManageOrdersController(manageOrderMock.Object, mapper.Object);
            var result = await orderController.ApproveBooking(1);

            Assert.Equal(1, (char)oneOrder.BookingInformation.Status);
        }

    }
}