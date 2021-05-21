using System;
using System.Collections.Generic;
using System.Linq;
using Catalog.Entities;

namespace Catalog.Repositories
{
    

    public class InMemItemsRespository : IItemsRespository //implements user repo depency inversion
    {
        private readonly List<Item> items = new()
        {
            new Item { Id = Guid.NewGuid(), Name = "Potion", Price = 9, CreateDate = DateTimeOffset.UtcNow }, //DateTimeOffSet= find the date right now
            new Item { Id = Guid.NewGuid(), Name = "Sword", Price = 20, CreateDate = DateTimeOffset.UtcNow },  //DateTimeOffSet= find the date right now
            new Item { Id = Guid.NewGuid(), Name = "Bronze Shield", Price = 18, CreateDate = DateTimeOffset.UtcNow },  //DateTimeOffSet= find the date right now
        };

        public IEnumerable<Item> GetItems()
        {
            return items;
        }

        public Item GetItem(Guid id)
        {
            {
                return items.Where(items => items.Id == id).SingleOrDefault();
                //where is from system.linq. It looks like it loops through the list.With single or default will return the item that matched id and not collection.default will be null
            }
        }

        public void CreateItem(Item item)
        {
            items.Add(item); 
        }

        public void UpdateItem(Item item)
        {
            var index = items.FindIndex(existingItem => existingItem.Id == item.Id);  //finds id of existing item and matches it to item
            items[index]= item; 
        }

        public void DeleteItem(Guid id)
        {
            var index= items.FindIndex(existingItem => existingItem.Id == id);
            items.RemoveAt(index);
        }
    }
}


/*This means that the variable or object can be assigned a value at the class scope or in a constructor only. You cannot change the value or
 reassign a value to a readonly variable or object in any other method except the constructo*/