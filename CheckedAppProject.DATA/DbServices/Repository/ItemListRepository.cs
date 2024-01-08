﻿using CheckedAppProject.DATA.CheckedAppDbContext;
using CheckedAppProject.DATA.Entities;
using Microsoft.EntityFrameworkCore;

namespace CheckedAppProject.DATA.DbServices.Repository;

public class ItemListRepository : IItemListRepository

{
    private UserItemContext _userItemContext;

    public ItemListRepository(UserItemContext userItemContext)
    {
        _userItemContext = userItemContext;
    }

    public async Task<IEnumerable<ItemList>> GetAllItemListsAsync()
    {
        var itemLists = await _userItemContext
            .ItemLists
            .Include(il => il.Items)
            .ToListAsync();

        return itemLists;
    }

    public async Task<IEnumerable<ItemList>> GetAllByUserIdAsync(Func<IQueryable<ItemList>, IQueryable<ItemList>> customQuery)
    {
        var query = _userItemContext.ItemLists.AsQueryable();

        query = customQuery(query);

        return await query
            .ToListAsync();
    }

    public async Task<ItemList?> GetItemListAsync(Func<IQueryable<ItemList>, IQueryable<ItemList>> customQuery)
    {
        var query = _userItemContext.ItemLists.AsQueryable();

        query = customQuery(query);

        return await query
                .Include(il => il.Items)
                .FirstOrDefaultAsync();
    }

    public async Task CreateItemList(ItemList itemList)
    {
        try
        {
            _userItemContext.ItemLists.Add(itemList);
            await _userItemContext.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> UpdateItemListAsync(ItemList itemList, int id)
    {
        var dbItemList = await _userItemContext.ItemLists.FirstOrDefaultAsync(il => il.ItemListId == id);

        if (dbItemList == null)
        {
            return false;
        }

        dbItemList.ItemListName = itemList.ItemListName ?? itemList.ItemListName;
        dbItemList.ItemListDestination = itemList.ItemListDestination ?? itemList.ItemListDestination;
        dbItemList.ItemListPublic = itemList.ItemListPublic;
        dbItemList.Date = itemList.Date ?? DateTime.Now;

        await _userItemContext.SaveChangesAsync();

        return true;
    }

    public async Task<ItemList> CopyItemList(int itemListid, int userId)
    {
        var dbItemList = await _userItemContext.ItemLists.FirstOrDefaultAsync(il => il.ItemListId == itemListid);

        if (dbItemList is null)
        {
            throw new Exception();
        }

        var copyItemList = new ItemList
        {
            Date = DateTime.UtcNow,
            ItemListDestination = dbItemList.ItemListDestination ?? "Destination",
            UserId = userId,
            ItemListName = dbItemList.ItemListName ?? "ItemList",
            UserItems = dbItemList.UserItems,
            ItemListPublic = false
        };

        try
        {
            _userItemContext.ItemLists.Add(copyItemList);
            await _userItemContext.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }

        return copyItemList;

    }

    public async Task<IEnumerable<ItemList>> GetAllItemListsByCityAndMonthAsync(string city, DateTime date)
    {
        var itemLists = await _userItemContext
            .ItemLists
            .Include(il => il.Items)
            .Where(il => il.ItemListDestination == city && il.Date.HasValue && il.Date.Value.Month == date.Month)
            .ToListAsync();

        return itemLists;
    }

    public async Task<bool> DeleteAsync(Func<IQueryable<ItemList>, IQueryable<ItemList>> customQuery)
    {
        var query = _userItemContext.ItemLists.AsQueryable();

        query = customQuery(query);

        var itemListToDelete = await query.FirstOrDefaultAsync();

        if (itemListToDelete != null)
        {
            _userItemContext.ItemLists.Remove(itemListToDelete);
            await _userItemContext.SaveChangesAsync();
            return true;
        }
        return false;
    }


}

