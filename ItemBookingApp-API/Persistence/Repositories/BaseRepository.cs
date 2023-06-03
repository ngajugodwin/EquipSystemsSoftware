using ItemBookingApp_API.Persistence.Contexts;

namespace ItemBookingApp_API.Persistence.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly ApplicationDbContext _context;
        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
