using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Domain.Services.Communication;
using ItemBookingApp_API.Resources.Query;

namespace ItemBookingApp_API.Domain.Services
{
    public interface ICategoryService
    {
        Task<PagedList<Category>> ListAsync(CategoryQuery categoryQuery);

        Task<IEnumerable<Category>> ListAsync();

        Task<CategoryResponse> GetCategoryByIdAsync(int categoryId);

        Task<CategoryResponse> SaveAsync(Category category);

        Task<CategoryResponse> UpdateAsync(int categoryId, Category category);

        Task<CategoryResponse> ActivateOrDeactivateCategory(int categoryId, bool categoryStatus);
    }
}
