﻿using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Resources.Query;

namespace ItemBookingApp_API.Domain.Repositories
{
    public interface IItemRepository
    {
        Task<PagedList<Item>> ListAsync(ItemQuery itemQuery, int itemTypeId);

        Task<IEnumerable<Item>> ListAsync(int categoryId, int[] itemIds = null);

        Task<IList<Item>> GetAvailableItems(int itemTypeId);

        bool SetItemState(IEnumerable<Item> itemsToChangeStatus, ItemState itemState);

        Task<bool> ItemIsInUse(int itemId);

        Task<bool> IsExist(string itemName, string serialNumber);
    }
}