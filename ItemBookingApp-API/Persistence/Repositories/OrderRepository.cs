using ItemBookingApp_API.Domain.Models.OrderAggregate;
using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Domain.Repositories;
using ItemBookingApp_API.Persistence.Contexts;
using ItemBookingApp_API.Resources.Query;
using Microsoft.EntityFrameworkCore;

namespace ItemBookingApp_API.Persistence.Repositories
{
    public class OrderRepository : BaseRepository, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }

        public void DeleteOrderAsync(Order order)
        {
            if (order != null)
            {
                //if (order != null && order.OrderItems.Count() > 0)
                //{
                //    order.OrderItems.ToList().Clear();
                //}

                _context.Orders.Remove(order);
            }
        }

        public async Task<Order> GetOrderByIdAsync(int id, string borrowerEmail)
        {
            var order =  await _context.Orders.Where(o => o.BorrowerEmail == borrowerEmail)
                .Include(x => x.OrderItems)
                .Include(x => x.DeliveryMethod).FirstOrDefaultAsync(x=> x.Id == id);

            return order ?? null;
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            var order = await _context.Orders.Where(o => o.Id == orderId)
                .Include(x => x.OrderItems)
                .Include(x => x.BookingInformation)
                .Include(x => x.DeliveryMethod).FirstOrDefaultAsync(x => x.Id == orderId);

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

        public async Task<PagedList<Order>> GetOrdersListForModerationAsync(OrderQuery orderQuery)
        {
            var orders = _context.Orders
                                .Include(d => d.DeliveryMethod)
                                .Include(o => o.OrderItems).ThenInclude(i => i.ItemOrdered)
                                .AsQueryable().AsNoTracking();

            if (orderQuery.OrderDate != null && orderQuery.OrderDate.HasValue)
            {
                orders = orders.Where(b => b.OrderDate.Date == orderQuery.OrderDate.Value.Date);
            }

            switch (orderQuery.Status)
            {
                case OrderStatus.Pending:
                    orders = orders.Where(b => b.Status == OrderStatus.Pending);
                    break;
                case OrderStatus.PaymentReceived:
                    orders = orders.Where(b => b.Status == OrderStatus.PaymentReceived);
                    break;
                case OrderStatus.PaymentFailed:
                    orders = orders.Where(b => b.Status == OrderStatus.PaymentReceived);
                    break;
                default:
                    break;
            }

            switch (orderQuery.BookingStatus)
            {
                case ApprovalStatus.Pending:
                    orders = orders.Where(b => b.BookingInformation.Status == ApprovalStatus.Pending);
                    break;
                case ApprovalStatus.Approved:
                    orders = orders.Where(b => b.BookingInformation.Status == ApprovalStatus.Approved);
                    break;
                case ApprovalStatus.Closed:
                    orders = orders.Where(b => b.BookingInformation.Status == ApprovalStatus.Closed);
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrWhiteSpace(orderQuery.SearchString))
            {
                orders = await SearchUser(orderQuery.SearchString, orders);
            }

            orders = orders.OrderByDescending(b => b.OrderDate);

            return await PagedList<Order>.CreateAsync(orders, orderQuery.PageNumber, orderQuery.PageSize);

        }

        private async Task<IQueryable<Order>> SearchUser(string name, IQueryable<Order> orders)
        {
            var user = await _context.Users.FirstOrDefaultAsync(b => b.FirstName.Contains(name)
                    || b.LastName.Contains(name));
                       

           if (user != null)
            {
                orders = orders.Where(x => x.BorrowerEmail.Contains(user.Email));
            }

            return orders;
        }
    }
}
