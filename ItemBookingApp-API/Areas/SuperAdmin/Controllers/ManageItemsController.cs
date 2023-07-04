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
        {    // find item by ID from database
            var item = await _genericRepository.FindAsync<Item>(x => x.Id == changeItemImageResource.ItemId);

            if (item == null)
                return BadRequest("Item not found!");


            if (!string.IsNullOrWhiteSpace(item.PublicId))
            {
                DeleteFileFromCloudindary(item.PublicId);
            }
            // create Cloudinary image upload result instance
            var file = changeItemImageResource.File;
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face") //transform image to capture relevant areas
                    };
                    //upload image to cloudinary 
                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }
            //update entity information with relevant data
            changeItemImageResource.Url = uploadResult.Url.ToString();
            changeItemImageResource.PublicId = uploadResult.PublicId;

            var itemToUpdate = _mapper.Map<ChangeItemImageResource, Item>(changeItemImageResource);

            item.Url = itemToUpdate.Url;
            item.PublicId = itemToUpdate.PublicId;

            try
            {    //store change to database
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
            
            //validate if an item exist with similar name
            var result = await _itemRepository.IsExist(saveItemResource.Name, saveItemResource.SerialNumber);

            if (result)
                return BadRequest("An item with the same name of serial number exist");

            var file = saveItemResource.File;

            //create an instance of cloudinary image upload result
            var uploadResult = new ImageUploadResult();

            if (file.Length> 0)
            {
                using(var stream = file.OpenReadStream())
                {   //set required attributes
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face") // transform image to capture relevant areas
                    };
                    //upload image to Cloudinary cloud storage
                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }
            // update item properties with relevant information
            saveItemResource.Url = uploadResult.Url.ToString();
            saveItemResource.PublicId = uploadResult.PublicId;


            var itemToSave = _mapper.Map<SaveItemResource, Item>(saveItemResource);

            try
            {   // set item to be active and available for user. Then save to Database
                itemToSave.IsActive = true; 
                itemToSave.ItemState = ItemState.Available;
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
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            if (itemId != updateItemResource.Id)
                return BadRequest("Id does not match request body");
            //get the item from database 
            var itemFromRepo = await _genericRepository.FindAsync<Item>(x => x.Id == itemId);

            if (itemFromRepo == null) return BadRequest("Item not found!");

            var item = _mapper.Map<UpdateItemResource, Item>(updateItemResource);

            //update properties with new information
            itemFromRepo.Name = item.Name;
            itemFromRepo.ItemTypeId = item.ItemTypeId;
            itemFromRepo.Description = item.Description;
            itemFromRepo.SerialNumber = item.SerialNumber;
            itemFromRepo.AvailableQuantity = item.AvailableQuantity;
            itemFromRepo.Price = item.Price;

            try
            {   // save changes to database
                _genericRepository.UpdateAsync<Item>(itemFromRepo);
                await _unitOfWork.CompleteAsync();

                var updatedItemToReturn = _mapper.Map<ItemResource>(itemFromRepo);
                //return updated information back to the client
                return Ok(updatedItemToReturn);
            }
            catch (Exception ex)
            {
                throw ex;
            }          
        }

        [HttpDelete("{itemId}")]
        public async Task<IActionResult> DeleteItemAsync(int itemId)
        {   //find item for deletion 
            var item = await _genericRepository.FindAsync<Item>(i => i.Id == itemId);

            if (item == null)
                return BadRequest("Item not found");

            if (item.ItemState == ItemState.NotAvailable)
            {   //delete and persist changes to database
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
