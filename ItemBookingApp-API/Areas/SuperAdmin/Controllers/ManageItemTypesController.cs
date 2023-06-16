using AutoMapper;
using ItemBookingApp_API.Areas.Resources.Item;
using ItemBookingApp_API.Areas.Resources.ItemType;
using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Domain.Repositories;
using ItemBookingApp_API.Extension;
using ItemBookingApp_API.Persistence.Repositories;
using ItemBookingApp_API.Services.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ItemBookingApp_API.Areas.SuperAdmin.Controllers
{
    [Route("super-admin/api/{categoryId}/[controller]")]
    [ApiController]
    [Authorize(Policy = PermissionSystemName.AccessSuperAdminArea)]
    public class ManageItemTypesController : ControllerBase
    {
        private readonly IGenericRepository _genericRepository;
        private readonly IItemTypeRepository _itemTypeRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ManageItemTypesController(IGenericRepository genericRepository, IItemTypeRepository itemTypeRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _genericRepository = genericRepository;
            _itemTypeRepository = itemTypeRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{itemTypeId}", Name = "GetItemTypeAsync")]
        public async Task<IActionResult> GetItemTypeAsync(int categoryId, int itemTypeId)
        {
            var item = await _genericRepository.FindAsync<ItemType>(x => x.Id == itemTypeId && x.CategoryId == categoryId);

            if (item == null)
                return BadRequest("Item type not found!");

            var itemTypeToReturn = _mapper.Map<ItemTypeResource>(item);

            return Ok(itemTypeToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> CreateItemTypeAsync([FromBody] SaveItemTypeResource saveItemTypeResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var category = await _genericRepository.FindAsync<Category>(c => c.Id == saveItemTypeResource.CategoryId);

            if (category == null || category.Id != saveItemTypeResource.CategoryId)
                return BadRequest("Requested resource is invalid");

            var result = await _itemTypeRepository.IsExist(saveItemTypeResource.Name);

            if (result)
                return BadRequest("An item type with the same name of serial number exist");

            var itemTypeToSave = _mapper.Map<SaveItemTypeResource, ItemType>(saveItemTypeResource);


            try
            {
                await _genericRepository.AddAsync<ItemType>(itemTypeToSave);
                await _unitOfWork.CompleteAsync();

                var itemTypeToReturn = _mapper.Map<ItemType, ItemTypeResource>(itemTypeToSave);

                return Ok(itemTypeToReturn);
            }
            catch (Exception ex)
            {
                throw;
            }
      
        }

        [HttpPut("{itemTypeId}")]
        public async Task<IActionResult> UpdateItemTypesync(int categoryId, int itemTypeId, [FromBody] UpdateItemTypeResource updateItemTypeResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            if (itemTypeId != updateItemTypeResource.Id)
                return BadRequest("Id does not match request body");

            var itemTypeFromRepo = await _genericRepository.FindAsync<ItemType>(x => x.Id == itemTypeId && x.CategoryId == categoryId);

            if (itemTypeFromRepo == null)
                return BadRequest("Item type not found!");

            var item = _mapper.Map<UpdateItemTypeResource, ItemType>(updateItemTypeResource);

            itemTypeFromRepo.Name = item.Name;
            itemTypeFromRepo.CategoryId = item.CategoryId;

            try
            {
                _genericRepository.UpdateAsync<ItemType>(itemTypeFromRepo);
                await _unitOfWork.CompleteAsync();

                var updatedItemTypeToReturn = _mapper.Map<ItemTypeResource>(itemTypeFromRepo);
                return Ok(updatedItemTypeToReturn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        [HttpPut("{itemTypeId}/changeItemTypeStatus")]
        public async Task<IActionResult> ActivateOrDisableItemType(int itemTypeId, [FromQuery] bool itemTypeStatus)
        {
            var itemType = await _genericRepository.FindAsync<ItemType>(i => i.Id == itemTypeId);

            if (itemType == null)
            {
                return BadRequest("Item type not found");
            }

            if (itemTypeStatus == true)
            {
                itemType.IsActive = true;

            }
            else
            {
                itemType.IsActive = false;
            }

            await _unitOfWork.CompleteAsync();

            var itemTypeData = _mapper.Map<ItemTypeResource>(itemType);

            return Ok(itemTypeData);
        }

        [HttpGet]
        public async Task<IActionResult> ListAsync(int categoryId, [FromQuery] ItemTypeQueryResource itemTypeQueryResource)
        {
            var itemTypeQuery = _mapper.Map<ItemTypeQueryResource, ItemTypeQuery>(itemTypeQueryResource);

            var itemTypes = await _itemTypeRepository.ListAsync(itemTypeQuery, categoryId);

            var itemTypesToReturn = _mapper.Map<IEnumerable<ItemTypeResource>>(itemTypes);

            Response.AddPagination(itemTypes.CurrentPage, itemTypes.PageSize, itemTypes.TotalCount, itemTypes.TotalPages);

            return Ok(itemTypesToReturn);
        }

        [HttpDelete("{itemTypeId}")]
        public async Task<IActionResult> DeleteItemTypeAsync(int itemTypeId)
        {
            var itemType = await _genericRepository.FindAsync<ItemType>(i => i.Id == itemTypeId);

            if (itemType == null)
            {
                return BadRequest("Item type not found");
            }

            _genericRepository.Remove<ItemType>(itemType);
            await _unitOfWork.CompleteAsync();

            var deletedItemType = _mapper.Map<ItemTypeResource>(itemType);

            return Ok(deletedItemType);
        }
    }
}
