using ItemBookingApp_API.Domain.Models.OrderAggregate;
using ItemBookingApp_API.Domain.Repositories;
using ItemBookingApp_API.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ItemBookingApp_API.Persistence.Repositories
{
    public class OrderRepository : BaseRepository, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Order> GetOrderByIdAsync(int id, string borrowerEmail)
        {
            var order =  await _context.Orders.Where(o => o.BorrowerEmail == borrowerEmail)
                .Include(x => x.OrderItems)
                .Include(x => x.DeliveryMethod).FirstOrDefaultAsync(x=> x.Id == id);

            return order ?? null;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string borrowerEmail)
        {
            var orders = await _context.Orders.Where(o => o.BorrowerEmail == borrowerEmail)
                .Include(x => x.DeliveryMethod)
                .Include(x => x.OrderItems)
                .OrderBy(x => x.OrderDate)
                .ToListAsync();


            return orders;
        }
    }
}
