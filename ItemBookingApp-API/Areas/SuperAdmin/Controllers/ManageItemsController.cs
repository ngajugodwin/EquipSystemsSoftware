using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ItemBookingApp_API.Areas.Resources.Item;
using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Domain.Repositories;
using ItemBookingApp_API.Extension;
using ItemBookingApp_API.Helpers;
using ItemBookingApp_API.Services.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ItemBookingApp_API.Areas.SuperAdmin.Controllers
{
    [Route("super-admin/api/{itemTypeId}/[controller]")]
    [ApiController]
    [Authorize(Policy = PermissionSystemName.AccessSuperAdminArea)]
    public class ManageItemsController : ControllerBase
    {
        private readonly IGenericRepository _genericRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private Cloudinary _cloudinary;
        public ManageItemsController(IMapper mapper, IUnitOfWork unitOfWork,
            IGenericRepository genericRepository,
            IItemRepository itemRepository,
            IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _genericRepository = genericRepository;
            _itemRepository = itemRepository;
            _cloudinaryConfig = cloudinaryConfig;

            Account acct = new Account(_cloudinaryConfig.Value.CloudName, _cloudinaryConfig.Value.ApiKey, _cloudinaryConfig.Value.ApiSecret);

            _cloudinary = new Cloudinary(acct);
        }

        [HttpGet("{itemId}", Name = "GetItemAsync")]
        public async Task<IActionResult> GetItemAsync(int itemId)
        {
            var item = await _genericRepository.FindAsync<Item>(x => x.Id == itemId);

            if (item == null)
                return BadRequest("Item not found!");

            var itemToReturn = _mapper.Map<ItemResource>(item);

            return Ok(itemToReturn);
        }

        private async void DeleteFileFromCloudindary(string publicId)
        {
            try
            {
                var deletionParams = new DeletionParams(publicId)
                {
                    ResourceType = ResourceType.Image,
                    PublicId = publicId,
                    Type = "upload",
                };

                var results = await _cloudinary.DestroyAsync(deletionParams);
            }
            catch (Exception ex)
            {
                throw ex;
            }

           
            
        }

        [HttpPut("{itemId}/changeItemImage")]
        public async Task<IActionResult> ChangeItemImage([FromForm] ChangeItemImageResource changeItemImageResource)
        {
            var item = await _genericRepository.FindAsync<Item>(x => x.Id == changeItemImageResource.ItemId);

            if (item == null)
                return BadRequest("Item not found!");


            if (!string.IsNullOrWhiteSpace(item.PublicId))
            {
                DeleteFileFromCloudindary(item.PublicId);
            }


            var file = changeItemImageResource.File;

            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }           


            changeItemImageResource.Url = uploadResult.Url.ToString();
            changeItemImageResource.PublicId = uploadResult.PublicId;


            var itemToUpdate = _mapper.Map<ChangeItemImageResource, Item>(changeItemImageResource);

            item.Url = itemToUpdate.Url;
            item.PublicId = itemToUpdate.PublicId;

            try
            {
                _genericRepository.UpdateAsync<Item>(item);
                await _unitOfWork.CompleteAsync();

                var updatedItemToReturn = _mapper.Map<ItemResource>(item);

                return Ok(updatedItemToReturn);
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }


        [HttpPost]
        public async Task<IActionResult> CreateItemAsync([FromForm] SaveItemResource saveItemResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var result = await _itemRepository.IsExist(saveItemResource.Name, saveItemResource.SerialNumber);

            if (result)
                return BadRequest("An item with the same name of serial number exist");

            //var itemToSave = _mapper.Map<SaveItemResource, Item>(saveItemResource);


            var file = saveItemResource.File;

            var uploadResult = new ImageUploadResult();

            if (file.Length> 0)
            {
                using(var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            saveItemResource.Url = uploadResult.Url.ToString();
            saveItemResource.PublicId = uploadResult.PublicId;


            var itemToSave = _mapper.Map<SaveItemResource, Item>(saveItemResource);

            try
            {
                itemToSave.IsActive = true;
                await _genericRepository.AddAsync<Item>(itemToSave);
                await _unitOfWork.CompleteAsync();

                var itemToReturn = _mapper.Map<Item, ItemResource>(itemToSave);

                return Ok(itemToReturn);
            }
            catch (Exception ex)
            {
                throw;
            }

            
        }

        [HttpPut("{itemId}")]
        public async Task<IActionResult> UpdateItemsync(int itemId, [FromBody] UpdateItemResource updateItemResource)
        {
            // return Ok();
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            if (itemId != updateItemResource.Id)
                return BadRequest("Id does not match request body");

            var itemFromRepo = await _genericRepository.FindAsync<Item>(x => x.Id == itemId);

            if (itemFromRepo == null)
                return BadRequest("Item not found!");

            var item = _mapper.Map<UpdateItemResource, Item>(updateItemResource);

            itemFromRepo.Name = item.Name;
            itemFromRepo.ItemTypeId = item.ItemTypeId;
            itemFromRepo.SerialNumber = item.SerialNumber;
            itemFromRepo.AvailableQuantity = item.AvailableQuantity;
            itemFromRepo.Price = item.Price;
            itemFromRepo.ItemState = (ItemState)updateItemResource.ItemStateId;

            try
            {
                _genericRepository.UpdateAsync<Item>(itemFromRepo);
                await _unitOfWork.CompleteAsync();

                var updatedItemToReturn = _mapper.Map<ItemResource>(itemFromRepo);

                return Ok(updatedItemToReturn);
            }
            catch (Exception ex)
            {
                throw ex;
            }          
        }

        [HttpDelete("{itemId}")]
        public async Task<IActionResult> DeleteItemAsync(int itemId)
        {
            var item = await _genericRepository.FindAsync<Item>(i => i.Id == itemId);

            if (item == null)
            {
                return BadRequest("Item not found");
            }

            if (item.ItemState == ItemState.NotAvailable)
            {
                _genericRepository.Remove<Item>(item);
                await _unitOfWork.CompleteAsync();

                var deletedItem = _mapper.Map<ItemResource>(item);

                return Ok(deletedItem);
            }

            return BadRequest("Only item marked as NOT AVAILABLE can be deleted");
        }

        [HttpPut("{itemId}/changeItemStatus")]
        public async Task<IActionResult> ActivateOrDisableItem(int itemId, bool itemStatus)
        {
            var item = await _genericRepository.FindAsync<Item>(i => i.Id == itemId);

            if (item == null)
            {
                return BadRequest("Item not found");
            }

            if (itemStatus == true)
            {
                item.IsActive = true;

            }
            else
            {
                item.IsActive = false;
            }

            await _unitOfWork.CompleteAsync();

            var itemData = _mapper.Map<ItemResource>(item);

            return Ok(itemData);
        }

        [HttpGet]
        public async Task<IActionResult> ListAsync(int itemTypeId, [FromQuery] ItemQueryResource itemQueryResource)
        {
            var itemQuery = _mapper.Map<ItemQueryResource, ItemQuery>(itemQueryResource);

            var items = await _itemRepository.ListAsync(itemQuery, itemTypeId);

            var itemsToReturn = _mapper.Map<IEnumerable<ItemResource>>(items);

            Response.AddPagination(items.CurrentPage, items.PageSize, items.TotalCount, items.TotalPages);

            return Ok(itemsToReturn);
        }

        [HttpGet("getAvailableItems")]
        public async Task<IActionResult> GetAvailableItems(int itemTypeId)
        {
            var items = await _itemRepository.GetAvailableItems(itemTypeId);

            var itemsToReturn = _mapper.Map<IEnumerable<ItemResource>>(items);

            return Ok(itemsToReturn);
        }

    }
}
