using System;
using Catalog.Entities;
using System.Collections.Generic;


namespace Catalog.Repositories
{
public interface IItemsRespository //extracted interface below  I at beginning is for interface
    {
        Item GetItem(Guid id);
        IEnumerable<Item> GetItems();

        void CreateItem(Item item);

        void UpdateItem(Item item);

        void DeleteItem(Guid id); 
    }

}