using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Domain.Repositories;
using ItemBookingApp_API.Persistence.Contexts;
using ItemBookingApp_API.Resources.CustomerQueries;
using ItemBookingApp_API.Resources.Query;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ItemBookingApp_API.Persistence.Repositories
{
    public class ItemRepository: BaseRepository, IItemRepository
    {

        public ItemRepository(ApplicationDbContext context)
            : base(context)
        {
        }


        public async Task<IList<Item>> GetAvailableItems(int itemTypeId)
        {
            return await _context.Items
               .Where(i => i.ItemTypeId == itemTypeId
               && i.IsActive == true
               && i.ItemState == ItemState.Available).ToListAsync();
        }

        public async Task<bool> IsExist(string itemName, string serialNumber)
        {
            var item = await _context.Items.AnyAsync(i => i.Name == itemName || i.SerialNumber == serialNumber);

            return item ? true : false;           
        }

        public async Task<bool> ItemIsInUse(int itemId)
        {
            var status = false;

            var result = await _context.Items.FirstOrDefaultAsync(x => x.Id == itemId);

            if (result != null)
            {
                switch (result.ItemState)
                {
                    case ItemState.NotAvailable:
                        status = false;
                        break;
                    case ItemState.Available:
                        status = false;
                        break;
                    case ItemState.Borrowed:
                        status = true;
                        break;
                    default:
                        status = false;
                        break;
                }
                return status;
            }
            return status;
        }

        public Task<List<Item>> GetItemsForCarouselDisplay()
        {
            var result =  _context.Items.Include(c => c.Category)
             .Include(i => i.ItemType)
             .Where(x => x.IsActive == true).OrderBy(x => x.Name).AsNoTracking();


            return result.ToListAsync();
        }


        public async Task<PagedList<Item>> GetAvailableItemsForCustomerListAsync(CustomerItemQuery customerItemQuery)
        {

            var result = _context.Items.Include(c => c.Category)
                .Include(i => i.ItemType)
                .Where(x => x.IsActive == true).OrderBy(x => x.Name).AsQueryable().AsNoTracking();

            if (customerItemQuery.CategoryId > 0)
            {
                result = result.Where(x => x.CategoryId == customerItemQuery.CategoryId);
            }

            if (customerItemQuery.ItemTypeId > 0)
            {
                result = result.Where(x => x.ItemTypeId == customerItemQuery.ItemTypeId);
            }            

            if (!string.IsNullOrWhiteSpace(customerItemQuery.Sort))
            {
                switch (customerItemQuery.Sort)
                {
                    case "priceAsc":
                        result = result.OrderByDescending(x => x.Price).Reverse();
                            break;
                    case "priceDesc":
                        result = result.OrderByDescending(x => x.Price);
                        break;
                    default:
                        result = result.OrderBy(x => x.Name);
                        break;
                }
            }

            if (!string.IsNullOrWhiteSpace(customerItemQuery.SearchString))
            {
                result = result.Where(x => x.Name.Contains(customerItemQuery.SearchString));
            }

           
            return await PagedList<Item>.CreateAsync(result, customerItemQuery.PageNumber, customerItemQuery.PageSize);
        }

        public async Task<Item> GetItemAsync(int itemId)
        {
            var result =  await _context.Items.Where(x => x.Id == itemId && x.IsActive == true)
                .Include(c => c.Category)
                .Include(i => i.ItemType).FirstAsync();

            return result;
        }

        public async Task<PagedList<Item>> ListAsync(ItemQuery itemQuery, int itemTypeId)
        {
            var items = Enumerable.Empty<Item>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(itemQuery.Status) && itemQuery.Status.ToLower() == "disabled")
            {
                items = _context.Items.IgnoreQueryFilters()
                    .Where(c => c.IsActive == false && c.ItemTypeId == itemTypeId)
                    .AsQueryable();
            }
            else
            {
                items = _context.Items.AsQueryable().Where(i => i.ItemTypeId == itemTypeId && i.IsActive == true);
            }


            if (!string.IsNullOrWhiteSpace(itemQuery.SearchString))
            {
                items = items.Where(c => c.Name.Contains(itemQuery.SearchString));
            }

            items = items.OrderByDescending(c => c.CreatedAt).AsNoTracking();

            return await PagedList<Item>.CreateAsync(items, itemQuery.PageNumber, itemQuery.PageSize);
        }      

        public async Task<IEnumerable<Item>> ListAsync(int[] itemIds = null)
        {
            
            if (itemIds == null)
                return await _context.Items.ToListAsync();

            return await _context.Items.Where(x => itemIds.Contains(x.Id)).ToListAsync();
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(int[] itemIds)
        {

            return await _context.Items.Where(x => itemIds.Contains(x.Id)).ToListAsync();
        }


        public bool SetItemState(IEnumerable<Item> itemsToChangeStatus, ItemState itemState)
        {
            if (itemsToChangeStatus.Count() > 0)
            {
                foreach (var item in itemsToChangeStatus)
                {
                    switch (itemState)
                    {
                        case ItemState.NotAvailable:
                            item.ItemState = ItemState.NotAvailable;
                            break;
                        case ItemState.Available:
                            item.ItemState = ItemState.Available;
                            break;
                        case ItemState.Borrowed:
                            item.ItemState = ItemState.Borrowed;
                            break;
                        default:
                            return false;
                    }
                }
                return true;
            }
            return false;
        }
        private IQueryable<Item> FilterByItemState(ItemState itemState, IQueryable<Item> items)
        {
            if (items != null)
            {
                switch (itemState)
                {
                    case ItemState.NotAvailable:
                        items = items.Where(x => x.ItemState == ItemState.NotAvailable);
                        break;
                    case ItemState.Available:
                        items = items.Where(x => x.ItemState == ItemState.Available);
                        break;
                    case ItemState.Borrowed:
                        items = items.Where(x => x.ItemState == ItemState.Borrowed);
                        break;
                    default:
                        items = items.Where((x) => x.ItemState == ItemState.Available);
                        break;
                }

                return items;
            }

            return null;
        }
    }
}
