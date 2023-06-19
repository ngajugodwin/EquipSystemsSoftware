using AutoMapper;
using ItemBookingApp_API.Areas.Resources.Category;
using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Domain.Services;
using ItemBookingApp_API.Extension;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ItemBookingApp_API.Areas.SuperAdmin.Controllers
{
    [Route("super-admin/api/[controller]")]
    [ApiController]
    public class ManageCategoriesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;

        public ManageCategoriesController(IMapper mapper, ICategoryService categoryService)
        {
            _mapper = mapper;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> ListAsync([FromQuery] CategoryQueryResource categoryQueryResource)
        {
            var categoryQuery = _mapper.Map<CategoryQueryResource, CategoryQuery>(categoryQueryResource);

            var categories = await _categoryService.ListAsync(categoryQuery);

            var categoriesToReturn = _mapper.Map<IEnumerable<CategoryResource>>(categories);

            Response.AddPagination(categories.CurrentPage, categories.PageSize, categories.TotalCount, categories.TotalPages);

            return Ok(categoriesToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategoryAsync([FromBody] SaveCategoryResource saveCategoryResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var categoryToSave = _mapper.Map<SaveCategoryResource, Category>(saveCategoryResource);

            var result = await _categoryService.SaveAsync(categoryToSave);

            if (!result.Success)
                return BadRequest(result.Message);

            var categoryToReturn = _mapper.Map<CategoryResource>(result.Resource);

            return Ok(categoryToReturn);
        }

        [HttpPut("{categoryId}")]
        public async Task<IActionResult> UpdateCategoryAsync(int categoryId, [FromBody] UpdateCategoryResource updateCategoryResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            if (categoryId != updateCategoryResource.Id)
                return BadRequest("Id does not match request body");

            var category = _mapper.Map<UpdateCategoryResource, Category>(updateCategoryResource);



            var result = await _categoryService.UpdateAsync(categoryId, category);

            if (!result.Success)
                return BadRequest(result.Message);

            var updatedCategoryToReturn = _mapper.Map<CategoryResource>(result.Resource);

            return Ok(updatedCategoryToReturn);
        }

        [HttpGet("{categoryId}", Name = "GetCategoryAsync")]
        public async Task<IActionResult> GetCategoryAsync(int categoryId)
        {
            var result = await _categoryService.GetCategoryByIdAsync(categoryId);

            if (!result.Success)
                return BadRequest(result.Message);

            var categoryToReturn = _mapper.Map<CategoryResource>(result.Resource);

            return Ok(categoryToReturn);
        }

        [HttpPut("{categoryId}/setCategoryStatus")]
        public async Task<IActionResult> SetCategoryStatus(int categoryId, [FromQuery] UpdateCategoryStatus updateCategoryStatus)
        {
            var result = await _categoryService.ActivateOrDeactivateCategory(categoryId, updateCategoryStatus.CategoryStatus);

            if (!result.Success)
                return BadRequest(result.Message);

            var updatedCategory = _mapper.Map<CategoryResource>(result.Resource);

            return Ok(updatedCategory);
        }

    }
}
