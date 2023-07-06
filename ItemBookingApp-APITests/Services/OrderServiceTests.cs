using Microsoft.VisualStudio.TestTools.UnitTesting;
using ItemBookingApp_API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemBookingApp_API.Domain.Models.OrderAggregate;

namespace ItemBookingApp_API.Services.Tests
{
    [TestClass()]
    public class OrderServiceTests
    {
        [TestMethod()]
        public void CreateOrderAsyncTest()
        {
            var orderItems = new List<OrderItem>();

            var item1 = new OrderItem
            {
                Id = 1,
                Price = 15,
                Quantity = 1,
                ItemOrdered = new ItemOrdered { ItemId = 1, Name = "Oven", PictureUrl = "" } 
            };

            orderItems.Add(item1);
            
            var order = new Order()
            {
                BorrowerEmail = "test@example.com",
                OrderDate = DateTime.Now,
                Id = 1,
                ShipToAddress = new Address("Test", "User", "2 Pivot Street", "Cov", "Coventry", "3PX"),
                OrderItems = orderItems,
                DeliveryMethod = new DeliveryMethod() { Id = 1, DeliveryTime = "2days", Price = 10, Description = "fast", ShortName = "UPS" }
            };

            Assert.IsNotNull(order);
        }
    }
}