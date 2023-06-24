using AutoMapper;
using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Domain.Models.OrderAggregate;
using ItemBookingApp_API.Domain.Services;
using ItemBookingApp_API.Resources.Basket;
using ItemBookingApp_API.Services.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace ItemBookingApp_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize(Policy = PermissionSystemName.HasUserRole)]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;
        private readonly ILogger<IPaymentService> _logger;
        private const string WhSecret = "whsec_ea1b53b6f62be763ae0796e5eacc7d51d940af99bf53311d908a1e93657d9afc";

        public PaymentsController(IPaymentService paymentService, IMapper mapper, ILogger<IPaymentService> logger)
        {
            _paymentService = paymentService;
            _mapper = mapper;
            _logger = logger;
        }

        [Authorize(Policy = PermissionSystemName.HasUserRole)]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasketResource>> CreateOrUpdatePaymentIntent(int basketId)
        {
            var result = await _paymentService.CreateOrUpdatePaymentIntent(basketId);

            var dataToReturn = _mapper.Map<CustomerBasket, CustomerBasketResource>(result);


            return Ok(dataToReturn);

        }

        [HttpPost("webhook")]
        public async Task<ActionResult> SetipeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], WhSecret);

            PaymentIntent intent;
            Order order;

            switch (stripeEvent.Type)
            {
                case "payment_intent.succeeded":
                    intent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Payment Succeeded: ", intent.Id);
                    order = await _paymentService.UpdateOrderPaymentSucceeded(intent.Id);
                    _logger.LogInformation("Order updated to payment received: ", order.Id);
                    break;
                case "payment_intent.payment_failed":
                    intent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Payment Failed: ", intent.Id);
                    order = await _paymentService.UpdateOrderPaymentFailed(intent.Id);
                    _logger.LogInformation("Order updated to payment failed: ", order.Id);
                    break;
                default:
                    break;
            }

            return new EmptyResult();
        }
    }
}
