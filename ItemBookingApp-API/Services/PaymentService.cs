using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Domain.Models.OrderAggregate;
using ItemBookingApp_API.Domain.Repositories;
using ItemBookingApp_API.Domain.Services;
using Stripe;

namespace ItemBookingApp_API.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository _genericRepository;
        private readonly IConfiguration _configuration;

        public PaymentService(IBasketRepository basketRepository,
            IItemRepository itemRepository,
            IUnitOfWork unitOfWork,
            IGenericRepository genericRepository, IConfiguration configuration)
        {
            _basketRepository = basketRepository;
            _itemRepository = itemRepository;
            _unitOfWork = unitOfWork;
            _genericRepository = genericRepository;
            _configuration = configuration;
        }

        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(int basketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];

            var basket = await _basketRepository.GetBasketAsync(basketId);

            var shippingPrice = 0m;

            if(basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _genericRepository.FindAsync<DeliveryMethod>(x => x.Id == basket.DeliveryMethodId);

                shippingPrice = deliveryMethod.Price;

            }

            foreach (var item in basket.Items)
            {
                var productItem = await _itemRepository.GetItemAsync(item.ItemId);

                if (productItem.Price != item.Item.Price)
                {
                    item.Item.Price = productItem.Price;
                }

            }

            var service = new PaymentIntentService();

            PaymentIntent intent;

            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)basket.Items.Sum(i => i.Quantity * (i.Item.Price * 100)) + (long)shippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };

                intent = await service.CreateAsync(options);
                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {

                    Amount = (long)basket.Items.Sum(i => i.Quantity * (i.Item.Price * 100)) + (long)shippingPrice * 100,
                };

                await service.UpdateAsync(basket.PaymentIntentId, options);
            }

            await _basketRepository.UpdateBasketAsync(basket);
          //  await _basketRepository.AddOneItemToExistingBasket(basket);
            return basket;


        }

        public async Task<Order> UpdateOrderPaymentFailed(string paymentIntentId)
        {

            var order = await _genericRepository.FindAsync<Order>(x => x.PaymentItentId == paymentIntentId);

            if (order == null) return null;


            order.Status = OrderStatus.PaymentFailed;
            await _unitOfWork.CompleteAsync();

            return order;
        }

        public async Task<Order> UpdateOrderPaymentSucceeded(string paymentIntentId)
        {
            var order = await _genericRepository.FindAsync<Order>(x => x.PaymentItentId == paymentIntentId);

            if (order == null) return null;

            order.Status = OrderStatus.PaymentReceived;

            _genericRepository.UpdateAsync<Order>(order);

            await _unitOfWork.CompleteAsync();

            return order;      

        }
    }
}
