using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Domain.Repositories;
using ItemBookingApp_API.Persistence.Contexts;
using ItemBookingApp_API.Resources.Query;
using Microsoft.EntityFrameworkCore;

namespace ItemBookingApp_API.Persistence.Repositories
{
    public class CategoryRepository : BaseRepository, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context)
            :base(context)
        {

        }

        public async Task<bool> IsValid(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);

            return (category != null && category.Status == EntityStatus.Active) ? true : false;
        }

        public async Task<bool> IsExist(string categoryName)
        {
            var category = await _context.Categories
                    .AnyAsync(c => c.Name.ToLower() == categoryName.ToLower());

            return (category) ? true : false;
        }

        public async Task<PagedList<Category>> ListAsync(CategoryQuery categoryQuery)
        {
            var categories = Enumerable.Empty<Category>().AsQueryable();

            if (categoryQuery.Status > 0)
            {
                switch (categoryQuery.Status)
                {
                    case EntityStatus.Pending:
                        categories = _context.Categories.Where(o => o.Status == EntityStatus.Pending);
                        break;
                    case EntityStatus.Active:
                        categories = _context.Categories.Where(o => o.Status == EntityStatus.Active);
                        break;
                    case EntityStatus.Disabled:
                        categories = _context.Categories.Where(o => o.Status == EntityStatus.Disabled);
                        break;
                    default:
                        categories = _context.Categories.AsQueryable();
                        break;
                }
            }
            else
            {
                categories = _context.Categories.AsQueryable();
            }

            if (!string.IsNullOrWhiteSpace(categoryQuery.SearchString))
            {
                categories = categories.Where(c => c.Name.Contains(categoryQuery.SearchString));
            }

            categories = categories.OrderByDescending(c => c.CreatedAt);

            return await PagedList<Category>.CreateAsync(categories, categoryQuery.PageNumber, categoryQuery.PageSize);
        }

        public async Task<IEnumerable<Category>> ListAsync()
        {
            return await _context.Categories.ToListAsync();
        }
    }
}
