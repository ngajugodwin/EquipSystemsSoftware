using AutoMapper;
using ItemBookingApp_API.Areas.Resources.ItemType;
using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Domain.Repositories;
using ItemBookingApp_API.Extension;
using ItemBookingApp_API.Resources.CustomerQueries;
using ItemBookingApp_API.Services.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ItemBookingApp_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = PermissionSystemName.HasUserRole)]
    public class ItemTypesController : ControllerBase
    {

        private readonly IGenericRepository _genericRepository;
        private readonly IItemTypeRepository _itemTypeRepository;
        private readonly IMapper _mapper;

        public ItemTypesController(IGenericRepository genericRepository, IItemTypeRepository itemTypeRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _itemTypeRepository = itemTypeRepository;
            _mapper = mapper;
        }

        [HttpGet("{itemTypeId}")]
        public async Task<IActionResult> GetItemTypeAsync(int itemTypeId)
        {
            var item = await _genericRepository.FindAsync<ItemType>(x => x.Id == itemTypeId);

            if (item == null)
                return BadRequest("Item type not found!");

            var itemTypeToReturn = _mapper.Map<ItemTypeResource>(item);

            return Ok(itemTypeToReturn);
        }

        [HttpGet]
        public async Task<IActionResult> CustomerListAsync([FromQuery] CustomerItemTypeQuery customerItemTypeQuery)
        {

            var itemTypes = await _itemTypeRepository.CustomerListAsync(customerItemTypeQuery);

            var itemTypesToReturn = _mapper.Map<IEnumerable<ItemTypeResource>>(itemTypes);

            Response.AddPagination(itemTypes.CurrentPage, itemTypes.PageSize, itemTypes.TotalCount, itemTypes.TotalPages);

            return Ok(itemTypesToReturn);
        }
    }
}
