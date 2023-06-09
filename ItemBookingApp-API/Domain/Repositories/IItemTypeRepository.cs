﻿using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Resources.Query;
using ItemBookingApp_API.Resources.CustomerQueries;

namespace ItemBookingApp_API.Domain.Repositories
{
    public interface IItemTypeRepository
    {
        Task<PagedList<ItemType>> ListAsync(ItemTypeQuery itemTypeQuery, int categoryId);
        Task<bool> IsExist(string itemTypeName);

        Task<PagedList<ItemType>> CustomerListAsync(CustomerItemTypeQuery customerItemTypeQuery);
    }
}
