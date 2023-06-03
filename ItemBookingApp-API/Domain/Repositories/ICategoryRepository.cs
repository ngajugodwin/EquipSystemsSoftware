using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Resources.Query;

namespace ItemBookingApp_API.Domain.Repositories
{
    public interface ICategoryRepository
    {
        Task<PagedList<Category>> ListAsync(CategoryQuery categoryQuery);

        Task<IEnumerable<Category>> ListAsync();

        Task<bool> IsExist(string categoryName);

        Task<bool> IsValid(int categoryId);
    }
}
