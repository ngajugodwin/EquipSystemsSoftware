﻿using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Domain.Models.OrderAggregate;
using ItemBookingApp_API.Domain.Repositories;
using ItemBookingApp_API.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ItemBookingApp_API.Persistence.Repositories
{
    public class BasketRepository : BaseRepository, IBasketRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IItemRepository _itemRepository;
        private readonly IGenericRepository _genericRepository;

        public BasketRepository(ApplicationDbContext context, IUnitOfWork unitOfWork, IItemRepository itemRepository, IGenericRepository genericRepository)
           : base(context)
        {
            _unitOfWork = unitOfWork;
            _itemRepository = itemRepository;
            _genericRepository = genericRepository;
        }

        public async Task<bool> DeleteBasketAsync(long userId, int basketId)
        {
            var result = await GetBasketAsync(userId, basketId);

            if (result != null)
            {

                foreach (var item in result.Items)
                {
                    _context.BasketItems.Remove(item);
                }

                _context.CustomerBaskets.Remove(result);
                await _unitOfWork.CompleteAsync();

                return true;
            }

            return false;

        }

        public async Task DeleteBasket(int basketId)
        {
            var result = await GetBasketAsync(basketId);

            if (result != null)
            {

                foreach (var item in result.Items)
                {
                    _context.BasketItems.Remove(item);
                }

                _context.CustomerBaskets.Remove(result);

                await _unitOfWork.CompleteAsync();
            }

        }

        public async Task<CustomerBasket> DeleteOneItemFromBasket(long userId, int basketId, int itemId)
        {
            var customerBasket = await GetBasketAsync(userId, basketId);

            if (customerBasket != null)
            {
                var basketItemForRemoval = customerBasket.Items.FirstOrDefault(x => x.ItemId == itemId);

                if (basketItemForRemoval != null)
                {
                    customerBasket.Items.Remove(basketItemForRemoval);
                    await _unitOfWork.CompleteAsync();
                }
                    
                return customerBasket;
            }

            return new CustomerBasket();
        }


        public async Task<CustomerBasket> UpdateDeliveryMethod(int basketId, int deliveryMethodId)
        {
            var customerBasket = await GetBasketAsync(basketId);

            var dm = await _genericRepository.FindAsync<DeliveryMethod>(x => x.Id == deliveryMethodId);

            if (customerBasket != null)
            {
                customerBasket.DeliveryMethodId = deliveryMethodId;
                customerBasket.ShippingPrice = dm.Price;
                await _unitOfWork.CompleteAsync();


                return customerBasket;
            }

            return null;
        }


        public async Task<CustomerBasket> AddOneItemToExistingBasket(long userId, int basketId, BasketItem basketItem)
        {

            // return new CustomerBasket();
            var customerBasket = await GetBasketAsync(userId, basketId);

            if (customerBasket != null)
            {
                var similarItem = customerBasket.Items.FirstOrDefault(x => x.ItemId == basketItem.ItemId);

                if (similarItem != null)
                {
                    similarItem.Quantity = basketItem.Quantity;
                }
                else
                {
                    customerBasket.Items.Add(new BasketItem
                    {
                        ItemId = basketItem.ItemId,
                        CustomerBasketId = basketItem.CustomerBasketId,
                        Quantity = basketItem.Quantity
                    });

                }


                await _unitOfWork.CompleteAsync();

                return customerBasket;
            }

            return new CustomerBasket();
        }

        public async Task<CustomerBasket> GetBasketAsync(long userId, int basketId)
        {

            var data = await _context.CustomerBaskets
               .Include(x => x.Items).ThenInclude(x => x.Item)
               .ThenInclude(x => x.ItemType)
               .Where(x => x.Id == basketId && x.UserId == userId).FirstAsync();

            return (data != null) ? data : new CustomerBasket();

        }

        public async Task<CustomerBasket> GetCurrentBasket(long userId)
        {
            var userBasket = await _context.CustomerBaskets.Include(x => x.Items).FirstOrDefaultAsync(x => x.UserId == userId);

            if (userBasket != null)
            {
                return userBasket;
            }

            return null;
        }

        public async Task<CustomerBasket> GetBasketAsync(int basketId)
        {
            var data = await _context.CustomerBaskets
                .Include(x => x.Items).ThenInclude(x => x.Item)
                .ThenInclude(x => x.ItemType)
                .Where(x => x.Id == basketId).FirstAsync();

            return (data != null) ? data : new CustomerBasket();

        }

        public async Task<CustomerBasket> AddBasketAsync(CustomerBasket basket)
        {
            var existingBasket = await _context.CustomerBaskets.FirstOrDefaultAsync(x => x.UserId == basket.UserId);

           
            if (existingBasket != null)
            {              

                var result = await AddOneItemToExistingBasket(basket.UserId, existingBasket.Id, basket.Items.Last());

                return result;
            }

            await _context.CustomerBaskets.AddAsync(basket);

            await _unitOfWork.CompleteAsync();

            return basket;
        }

        public async Task<CustomerBasket> IncreaseItemQuantity(long userId, int basketId, int itemId)
        {
            var customerBasket = await GetBasketAsync(userId, basketId);

            var itemFromRepo = await _itemRepository.GetItemAsync(itemId);


            if (customerBasket != null)
            {
                var item = customerBasket.Items.FirstOrDefault(x => x.ItemId == itemId);
               

                if (item != null)
                {
                    if (item.Quantity == itemFromRepo.AvailableQuantity)
                    {
                        // reached limit. do not increase item quantity
                        return customerBasket;
                    }

                   item.Quantity++;                  

                    await _unitOfWork.CompleteAsync();
                }

                return customerBasket;
            }

            return new CustomerBasket();
        }

        public async Task<CustomerBasket> DecreaseItemQuantity(long userId, int basketId, int itemId)
        {
            var customerBasket = await GetBasketAsync(userId, basketId);

            if (customerBasket != null)
            {
                var item = customerBasket.Items.FirstOrDefault(x => x.ItemId == itemId);

                if (item != null)
                {
                    if (item.Quantity <= 0)
                    {
                        customerBasket.Items.Remove(new BasketItem
                        {
                            ItemId = itemId,
                            CustomerBasketId = basketId,
                            Quantity = 1,
                        });
                    } 
                    else
                    {
                        item.Quantity--;

                    }

                    await _unitOfWork.CompleteAsync();
                }

                return customerBasket;
            }

            return new CustomerBasket();
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basketToUpdate)
        {
            var existingBasket = await GetBasketAsync(basketToUpdate.Id);
            // _context.CustomerBaskets.FirstOrDefaultAsync(b => b.Id == basketToUpdate.Id);


            if (existingBasket != null)
            {
                if (existingBasket.Items.Count() > 0)
                {
                    foreach (var newItem in basketToUpdate.Items)
                    {
                        foreach (var oldItem in existingBasket.Items)
                        {
                            if (newItem.ItemId == oldItem.ItemId && newItem.Quantity != oldItem.Quantity)
                            {
                                oldItem.Quantity = newItem.Quantity;
                            }
                        }

                    }
                }
                _context.CustomerBaskets.Update(existingBasket);

                await _unitOfWork.CompleteAsync();

                return basketToUpdate;
            }

            return new CustomerBasket();
        }

       
    }
}
