using System.Runtime.InteropServices.JavaScript;
using System.Text.RegularExpressions;
using ShoppingList.Application.Interfaces;
using ShoppingList.Domain.Models;

namespace ShoppingList.Application.Services;

public class ShoppingListService : IShoppingListService
{
    private ShoppingItem[] _items;
    private int _nextIndex;

    public ShoppingListService()
    {
        // Initialize with demo data for UI demonstration
        // TODO: Students can remove or comment this out when running unit tests
        _items = [];
        _nextIndex = 0; // We have 4 demo items initialized
    }

    public IReadOnlyList<ShoppingItem> GetAll()
    {
        // TODO: Students - Return all items from the array (up to _nextIndex)
        var items = new ShoppingItem[_items.Length];
        for (int i = 0; i < _nextIndex; i++)
        {
            items[i] = _items[i];
        }
        return items;
    }

    public ShoppingItem? GetById(string id)
    {
        // TODO: Students - Find and return the item with the matching id
        var itemById =  _items.FirstOrDefault(x => x.Id == id);
        return itemById;
    }

    public ShoppingItem? Add(string name, int quantity, string? notes)
    {
        var shoppingItem = new ShoppingItem
        {
            Name = name,
            Quantity = quantity,
            Notes = notes
        };
        if (_items.Length == 0)
        {
            Array.Resize(ref _items, _items.Length + 1);
        }
        else if (_items.Length == _nextIndex)
        {
            Array.Resize(ref _items, _items.Length + 1);
        }
        _items[_nextIndex] = shoppingItem;
        _nextIndex++;
        return shoppingItem;
    }

    public ShoppingItem? Update(string id, string name, int quantity, string? notes)
    {
        // TODO: Students - Implement this method
        // Return the updated item, or null if not found
        return null;
    }

    public bool Delete(string id)
    {
        // TODO: Students - Implement this method
        // Return true if deleted, false if not found
        if(!_items.Any(x => x.Id == id))  return false;
        
        var newItemList = new ShoppingItem[_items.Length];
        _nextIndex = 0;

        foreach (var item in _items)
        {
            if (item.Id != id)
            {
                newItemList[_nextIndex] = item;
                _nextIndex++;
            }
        }
        
        
        return true;
    }

    public IReadOnlyList<ShoppingItem> Search(string query)
    {
        // TODO: Students - Implement this method
        // Return the filtered items
        // var regex = new Regex(query.ToLower());
        // var filteredItems = (from item in _items
        //     where item.Notes != null && regex.Match(item.Notes?.ToLower()).Success || regex.Match(item.Name.ToLower()).Success
        //         select item).ToArray();
        if (query == string.Empty || query == null)
        {
            return _items;
        }

        var filteredItems = new  ShoppingItem[_items.Length];
        int index = 0;
        foreach (var item in _items)
        {
            
            if (item.Name.Contains(query) || item.Notes.Contains(query) && item.Notes != null)
            {
                filteredItems[index] =  item;
            }
            index++;
        }
        return filteredItems;
    }

    public int ClearPurchased()
    {
        // TODO: Students - Implement this method
        // Return the count of removed items
        return 0;
    }

    public bool TogglePurchased(string id)
    {
        // TODO: Students - Implement this method
        // Return true if successful, false if item not found
        return false;
    }

    public bool Reorder(IReadOnlyList<string> orderedIds)
    {
        // TODO: Students - Implement this method
        // Return true if successful, false otherwise
        return false;
    }

    private ShoppingItem[] GenerateDemoItems()
    {
        var items = new ShoppingItem[5];
        items[0] = new ShoppingItem
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Dishwasher tablets",
            Quantity = 1,
            Notes = "80st/pack - Rea",
            IsPurchased = false
        };
        items[1] = new ShoppingItem
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Ground meat",
            Quantity = 1,
            Notes = "2kg - origin Sweden",
            IsPurchased = false
        };
        items[2] = new ShoppingItem
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Apples",
            Quantity = 10,
            Notes = "Pink Lady",
            IsPurchased = false
        };
        items[3] = new ShoppingItem
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Toothpaste",
            Quantity = 1,
            Notes = "Colgate",
            IsPurchased = false
        };
        return items;
    }
}