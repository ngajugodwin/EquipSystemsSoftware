using AutoMapper;
using ItemBookingApp_API.Areas.Resources.Category;
using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Domain.Repositories;
using ItemBookingApp_API.Extension;
using ItemBookingApp_API.Resources.Basket;
using ItemBookingApp_API.Resources.CustomerQueries;
using ItemBookingApp_API.Services.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ItemBookingApp_API.Controllers
{

    [Route("api/{userId}/[controller]")]
    [ApiController]
    [Authorize(Policy = PermissionSystemName.HasUserRole)]
    public class BasketController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBasketRepository _basketRepository;
        public BasketController(IMapper mapper, IBasketRepository basketRepository)
        {
            _mapper = mapper;
            _basketRepository = basketRepository;
        }

        [HttpGet("{basketId}")]
        public async Task<IActionResult> GetBasketById(long userId, int basketId)
        {
            var basket = await _basketRepository.GetBasketAsync(userId, basketId);

            var backetToReturn = _mapper.Map<CustomerBasket, CustomerBasketResource>(basket);

            return Ok(backetToReturn ?? new CustomerBasketResource(basketId, userId));
        }

        [HttpPost]
        public async Task<IActionResult> AddBasketAsync(SaveCustomerBasketResource saveCustomerBasketResource)
        {
            var customerBasket = _mapper.Map<SaveCustomerBasketResource, CustomerBasket>(saveCustomerBasketResource);

            var basket = await _basketRepository.AddBasketAsync(customerBasket);

            var backetToReturn = _mapper.Map<CustomerBasket, CustomerBasketResource>(basket);

            return Ok(backetToReturn);
        }

        [HttpPut("{basketId}")]
        public async Task<IActionResult> IncreaseOrDecreaseItemQuantityAsync(long userId, int basketId, IncreaseDecreaseResource data)
        {
            var result = new CustomerBasket();

            if (data.Status)
            {
               result = await _basketRepository.IncreaseItemQuantity(userId, basketId, data.ItemId);
            } 
            else
            {
                result = await _basketRepository.DecreaseItemQuantity(userId, basketId, data.ItemId);
            }

            var backetToReturn = _mapper.Map<CustomerBasket, CustomerBasketResource>(result);

            return Ok(backetToReturn);
        }

        //[HttpPut("{basketId}")]
        //public async Task<IActionResult> UpdateBasketAsync(long userId, int basketId, UpdateCustomerBasketResource updateCustomerBasketResource)
        //{
        //    var customerBasketToUpdate = _mapper.Map<UpdateCustomerBasketResource, CustomerBasket>(updateCustomerBasketResource);

        //    var basket = await _basketRepository.UpdateBasketAsync(customerBasketToUpdate);

        //    var basketToReturn = _mapper.Map<CustomerBasket, CustomerBasketResource>(basket);

        //    return Ok(basketToReturn ?? new CustomerBasketResource(basketId, userId));
        //}

        [HttpPut("{basketId}/removeOneItemFromBasket/{itemId}")]
        public async Task<IActionResult> RemoveOneItemFromBasketAsync(long userId, int basketId, int itemId)
        {
            var result = await _basketRepository.DeleteOneItemFromBasket(userId, basketId, itemId);

            var backetToReturn = _mapper.Map<CustomerBasket, CustomerBasketResource>(result);

            return Ok(backetToReturn);
        }


        [HttpDelete("{basketId}")]
        public async Task<IActionResult> DeleteBasketAsync(long userId, int basketId)
        {
            var result  = await _basketRepository.DeleteBasketAsync(userId, basketId);

            return Ok(result);
        }
    }
}
