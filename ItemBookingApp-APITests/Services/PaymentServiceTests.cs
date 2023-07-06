using Microsoft.VisualStudio.TestTools.UnitTesting;
using ItemBookingApp_API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemBookingApp_API.Domain.Models;
using Stripe;
using Microsoft.Extensions.Configuration;

namespace ItemBookingApp_API.Services.Tests
{
    [TestClass()]
    public class PaymentServiceTests
    {
        private readonly IConfiguration _configuration;

        public PaymentServiceTests(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [TestMethod()]
        public void CreateOrUpdatePaymentIntentTest()
        {
       //     StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
            var basketItem = new BasketItem { CustomerBasketId = 1, ItemId = 1, Quantity = 1};

            var customerBasket = new CustomerBasket
            {
                ClientSecret = "RR333",
                DeliveryMethodId = 1,
                Id = 2,
                UserId = 1,
                PaymentIntentId = "xyr321uehriu8392",
                ShippingPrice = 10,
                
            };

            customerBasket.Items.Add(basketItem);

            basketItem = null;


            //var service = new PaymentIntentService();
            //PaymentIntent intent;

            //var options = new PaymentIntentCreateOptions
            //{
            //    Amount = (long)customerBasket.Items.Sum(i => i.Quantity * (i.Item.Price * 100)) + (long)customerBasket.ShippingPrice * 100,
            //    Currency = "usd",
            //    PaymentMethodTypes = new List<string> { "card" }
            //};

            //intent = await service.CreateAsync(options);

            Assert.IsNotNull(basketItem);
        }
    }
}