using AutoMapper;
using ItemBookingApp_API.Areas.Resources.Category;
using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Domain.Services;
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
    public class CategoriesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;

        public CategoriesController(IMapper mapper, ICategoryService categoryService)
        {
            _mapper = mapper;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> ListAsync([FromQuery] CustomerCategoryQuery customerCategoryQuery)
        {
            var categories = await _categoryService.CustomerListAsync(customerCategoryQuery);

            var categoriesToReturn = _mapper.Map<IEnumerable<CategoryResource>>(categories);

            Response.AddPagination(categories.CurrentPage, categories.PageSize, categories.TotalCount, categories.TotalPages);

            return Ok(categoriesToReturn);
        }

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetCategoryAsync(int categoryId)
        {
            var result = await _categoryService.GetCategoryByIdAsync(categoryId);

            if (!result.Success)
                return BadRequest(result.Message);

            var categoryToReturn = _mapper.Map<CategoryResource>(result.Resource);

            return Ok(categoryToReturn);
        }
    }
}
