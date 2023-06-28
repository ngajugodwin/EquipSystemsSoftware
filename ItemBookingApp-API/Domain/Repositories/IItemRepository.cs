using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Resources.CustomerQueries;
using ItemBookingApp_API.Resources.Query;

namespace ItemBookingApp_API.Domain.Repositories
{
    public interface IItemRepository
    {
        Task<List<Item>> GetItemsForCarouselDisplay();
        Task<Item> GetItemAsync(int itemId);
        Task<PagedList<Item>> GetAvailableItemsForCustomerListAsync(CustomerItemQuery customerItemQuery);
        Task<PagedList<Item>> ListAsync(ItemQuery itemQuery, int itemTypeId);

        Task<IEnumerable<Item>> ListAsync(int[] itemIds = null);

        Task<IList<Item>> GetAvailableItems(int itemTypeId);

        bool SetItemState(IEnumerable<Item> itemsToChangeStatus, ItemState itemState);

        Task<bool> ItemIsInUse(int itemId);

        Task<bool> IsExist(string itemName, string serialNumber);

        Task<IEnumerable<Item>> GetItemsAsync(int[] itemIds);

    }
}
