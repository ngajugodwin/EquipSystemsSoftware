using AutoMapper;
using Castle.Core.Logging;
using ItemBookingApp_API.Areas.SuperAdmin;
using ItemBookingApp_API.Controllers;
using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Domain.Services;
using ItemBookingApp_API.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemBookingAppTest
{
    public class PaymentProcessing_Test
    {
        public Mock<IPaymentService> mock = new Mock<IPaymentService>();
        public Mock<IMapper> mapper = new Mock<IMapper>();
        public Mock<ILogger<IPaymentService>> logger = new Mock<ILogger<IPaymentService>>();

        [Fact]
        public async void CreateCustomerPaymentIntent_Test()
        {
            // initialise basket item using fake data
            var basketItem = new BasketItem
            {
                ItemId = 1,
                Quantity = 2,
                CustomerBasketId = 1,
                Item = new Item
                {
                    Id = 1,
                    Name = "Bicycle",
                    Price = 12,
                    Description = "Test Bicycle"
                }
            };

            var basketItems = new List<BasketItem> { basketItem };

            // create test customer basket which will be using for payment proccessing
            var customerBasket = new CustomerBasket
            {
                Id = 2,
                ClientSecret = "ejueiuho89hwe8",
                DeliveryMethodId = 2,
                ShippingPrice = 10,
                Items = basketItems
            };

            // mock up the creation of the paymentIntent function using the customer basket
            mock.Setup(p => p.CreateOrUpdatePaymentIntent(customerBasket.Id)).ReturnsAsync(customerBasket);

            PaymentsController paymentController = new PaymentsController(mock.Object, mapper.Object, logger.Object);

            // test the payment intent function from the payment controller using the fake data prepared
            var result = await paymentController.CreateOrUpdatePaymentIntent(customerBasket.Id);

            Assert.NotNull(result); // Assert that a payment was successful by returning a valid basket
        }


    }
}
