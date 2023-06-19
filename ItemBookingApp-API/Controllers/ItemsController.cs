using AutoMapper;
using ItemBookingApp_API.Areas.Resources.Item;
using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Domain.Repositories;
using ItemBookingApp_API.Extension;
using ItemBookingApp_API.Persistence.Repositories;
using ItemBookingApp_API.Resources.CustomerQueries;
using ItemBookingApp_API.Services.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ItemBookingApp_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = PermissionSystemName.HasUserRole)]
    public class ItemsController : ControllerBase
    {
        private readonly IGenericRepository _genericRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;

        public ItemsController(IGenericRepository genericRepository, IItemRepository itemRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _itemRepository = itemRepository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> ListAsync([FromQuery] CustomerItemQuery customerItemQuery)
        {
            var items = await _itemRepository.GetAvailableItemsForCustomerListAsync(customerItemQuery);

            var itemsToReturn = _mapper.Map<IEnumerable<Item>, IEnumerable <ItemToReturnDto>>(items);


            Response.AddPagination(items.CurrentPage, items.PageSize, items.TotalCount, items.TotalPages);            

            return Ok(itemsToReturn);
        }

        [HttpGet("{itemId}")]
        public async Task<IActionResult> GetItemAsync(int itemId)
        {
            var item = await _itemRepository.GetItemAsync(itemId);

            if (item == null)
                return BadRequest("Item not found!");

            var itemToReturn = _mapper.Map<Item, ItemToReturnDto>(item);

            return Ok(itemToReturn);
        }

    }
}
