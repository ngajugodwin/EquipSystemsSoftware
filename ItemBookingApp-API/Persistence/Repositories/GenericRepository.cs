using ItemBookingApp_API.Domain.Repositories;
using ItemBookingApp_API.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ItemBookingApp_API.Persistence.Repositories
{
    public class GenericRepository : BaseRepository, IGenericRepository
    {

        public GenericRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task AddAsync<T>(T entity) where T : class
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public async Task AddRangeAsync<T>(List<T> entity) where T : class
        {
            await _context.Set<T>().AddRangeAsync(entity);
        }

        public async Task<T> FindAsync<T>(Expression<Func<T, bool>> predicate, bool ignoreQueryFilters = false) where T : class
        {
            var query = _context.Set<T>().AsQueryable();

            if (ignoreQueryFilters)
                query = query.IgnoreQueryFilters();

            return await query.FirstOrDefaultAsync(predicate);
        }

        public async Task<ICollection<T>> ListAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            var query = _context.Set<T>().AsQueryable();

            return await query.Where<T>(predicate).ToListAsync();
        }

        public void Remove<T>(T entity) where T : class
        {
            _context.Set<T>().Remove(entity);
        }

        public void UpdateAsync<T>(T entity) where T : class
        {
            _context.Set<T>().Update(entity);
        }
    }
}
