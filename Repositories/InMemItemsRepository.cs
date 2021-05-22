using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<Item>> GetItemsAsync()
        {   //FromResult create a  task that has already completed and introduce the value of items collection into task
            return await Task.FromResult(items);
        }

        public async Task<Item> GetItemAsync(Guid id)
        {
            {
                var item= items.Where(items => items.Id == id).SingleOrDefault();
                //where is from system.linq. It looks like it loops through the list.With single or default will return the item that matched id and not collection.default will be null
                return await Task.FromResult(item);
            }
        }

        public async Task CreateItemAsync(Item item)
        {
            items.Add(item); 
            await Task.CompletedTask;
            //above means create a task which has completed and return task
        }

        public async Task UpdateItemAsync(Item item)
        {
            var index = items.FindIndex(existingItem => existingItem.Id == item.Id);  //finds id of existing item and matches it to item
            items[index]= item; 
            await Task.CompletedTask;
        }

        public async Task DeleteItemAsync(Guid id)
        {
            var index= items.FindIndex(existingItem => existingItem.Id == id);
            items.RemoveAt(index);
            await Task.CompletedTask;
        }
    }
}


/*This means that the variable or object can be assigned a value at the class scope or in a constructor only. You cannot change the value or
 reassign a value to a readonly variable or object in any other method except the constructo*/