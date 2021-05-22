using System;
using System.Collections.Generic;
using Catalog.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Repositories
{
    public class MongoDbItemsRepository : IItemsRespository
    {
        private const string databaseName= "catalog";

        private const string collectionName = "items";
        private readonly IMongoCollection<Item> itemsCollection;

        //needed for returning a single item
        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;


        public MongoDbItemsRepository(IMongoClient mongoClient)
        {   //collection now matter which databse we use,they will be automatically created for me
            //dotnet add package MongoDb.Driver
            IMongoDatabase database= mongoClient.GetDatabase(databaseName); //reference to database
            itemsCollection = database.GetCollection<Item>(collectionName);
        }
        public void CreateItem(Item item)
        {
            itemsCollection.InsertOne(item);
            
        }

        public void DeleteItem(Guid id)
        {   
            //search through databse where item id matches id
            var filter = filterBuilder.Eq(items => items.Id, id);
            itemsCollection.DeleteOne(filter);
            //deletes the filtered item
        }

        public Item GetItem(Guid id)
        {
            //filters items and returns one where id matches item's id
            var filter = filterBuilder.Eq(items => items.Id, id);
            return itemsCollection.Find(filter).SingleOrDefault();
            //returns the item or a default

        }

        public IEnumerable<Item> GetItems()
        {
           return itemsCollection.Find(new BsonDocument()).ToList(); //to list will list all items 
        }

        public void UpdateItem(Item item)
        {   
            //finds existing item, where it matches id, replace with item.Id params
            var filter = filterBuilder.Eq(existingItem => existingItem.Id, item.Id);
            itemsCollection.ReplaceOne(filter, item);
        }
    }
}