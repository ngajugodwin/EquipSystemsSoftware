using ItemBookingApp_API.Domain.Models.OrderAggregate;
using ItemBookingApp_API.Domain.Repositories;
using ItemBookingApp_API.Domain.Services;

namespace ItemBookingApp_API.Services
{
    public class OrderService : IOrderService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IBasketRepository _basketRepository;
        private readonly IGenericRepository _genericRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IItemRepository itemRepository, 
            IOrderRepository orderRepository,
            IBasketRepository basketRepository, IGenericRepository genericRepository,
            IUnitOfWork unitOfWork)
        {
            _itemRepository = itemRepository;
            _orderRepository = orderRepository;
            _basketRepository = basketRepository;
            _genericRepository = genericRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Order> CreateOrderAsync(string borrowerEmail, int deliveryMethodId, int basketId, Address shippingAddress)
        {
            // get basket
            var basket = await _basketRepository.GetBasketAsync(basketId);

            var items = new List<OrderItem>();

            //create order item
            foreach (var item in basket.Items)
            {
                var productItem = await _itemRepository.GetItemAsync(item.ItemId);

                var itemOrdered = new ItemOrdered(productItem.Id, productItem.Name, productItem.Url);

                var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);

                items.Add(orderItem);
            }

            // get delivery method
            var deliveryMethods = await _genericRepository.FindAsync<DeliveryMethod>(x => x.Id == deliveryMethodId);

            // calc subtotal
            var subTotal = items.Sum(item => item.Price * item.Quantity);

            var order = new Order(items, borrowerEmail, shippingAddress, deliveryMethods, subTotal);

            if (basket.PaymentIntentId != null)
                order.PaymentItentId = basket.PaymentIntentId;

            await _genericRepository.AddAsync<Order>(order);

            try
            {
                //save to db and return order
                
                await _unitOfWork.CompleteAsync();

                //delete basket
             //   await _basketRepository.DeleteBasket(basketId);
                
               // return processed order to client
                return order;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        public async Task<IEnumerable<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            var result = await _genericRepository.ListAsync<DeliveryMethod>();

            //var d = result.GetType().tol;

            ////return (IReadOnlyList)result;
            //return d.Current.;

            return result;
        }

        public async Task<Order> GetOrderByIdAsync(int id, string borrowerEmail)
        {
            return await _orderRepository.GetOrderByIdAsync(id, borrowerEmail);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string borrowerEmail)
        {

            var result = await _orderRepository.GetOrdersForUserAsync(borrowerEmail);

            return result;
        }
    }
}
