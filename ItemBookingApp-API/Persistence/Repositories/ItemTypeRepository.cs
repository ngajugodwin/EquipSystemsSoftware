﻿using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Persistence.Contexts;
using ItemBookingApp_API.Resources.Query;
using Microsoft.EntityFrameworkCore;
using ItemBookingApp_API.Domain.Repositories;
using ItemBookingApp_API.Resources.CustomerQueries;

namespace ItemBookingApp_API.Persistence.Repositories
{
    public class ItemTypeRepository : BaseRepository, IItemTypeRepository
    {
        public ItemTypeRepository(ApplicationDbContext context)
         : base(context)
        {
        }

        public async Task<bool> IsExist(string itemTypeName)
        {
            var itemType = await _context.ItemTypes.AnyAsync(i => i.Name == itemTypeName);

            return itemType ? true : false;
        }

        public async Task<PagedList<ItemType>> CustomerListAsync(CustomerItemTypeQuery customerItemTypeQuery)
        {

            var itemTypes = _context.ItemTypes.AsQueryable()
                .Where(i => i.IsActive == true).AsNoTracking();


            return await PagedList<ItemType>.CreateAsync(itemTypes, customerItemTypeQuery.PageNumber, customerItemTypeQuery.PageSize);
        }
        

        
        public async Task<PagedList<ItemType>> ListAsync(ItemTypeQuery itemTypeQuery, int categoryId)
        {
            var itemTypes = Enumerable.Empty<ItemType>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(itemTypeQuery.Status) && itemTypeQuery.Status.ToLower() == "disabled")
            {
                itemTypes = _context.ItemTypes.Where(c => c.IsActive == false && c.CategoryId == categoryId)
                    .AsQueryable();
            }
            else
            {
                itemTypes = _context.ItemTypes.AsQueryable().Where(i => i.CategoryId == categoryId && i.IsActive == true);
            }


            if (!string.IsNullOrWhiteSpace(itemTypeQuery.SearchString))
            {
                itemTypes = itemTypes.Where(c => c.Name.Contains(itemTypeQuery.SearchString));
            }

            itemTypes = itemTypes.OrderByDescending(c => c.CreatedAt).AsNoTracking();

            return await PagedList<ItemType>.CreateAsync(itemTypes, itemTypeQuery.PageNumber, itemTypeQuery.PageSize);
        }

       
    }
}
