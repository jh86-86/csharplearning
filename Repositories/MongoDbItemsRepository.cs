using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task CreateItemAsync(Item item)  //added async and Task removed void
        {
            //itemsCollection.InsertOne(item);
            await itemsCollection.InsertOneAsync(item); //asynchronous version with task
            
        }

        public async Task DeleteItemAsync(Guid id)
        {   
            //search through databse where item id matches id
            var filter = filterBuilder.Eq(items => items.Id, id);
            await itemsCollection.DeleteOneAsync(filter);
            //deletes the filtered item
        }

        public async Task<Item> GetItemAsync(Guid id)
        {
            //filters items and returns one where id matches item's id
            var filter = filterBuilder.Eq(items => items.Id, id);
            return await itemsCollection.Find(filter).SingleOrDefaultAsync();
            //returns the item or a default

        }

        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
           return await itemsCollection.Find(new BsonDocument()).ToListAsync(); //to list will list all items 
        }

        public async Task UpdateItemAsync(Item item)
        {   
            //finds existing item, where it matches id, replace with item.Id params
            var filter = filterBuilder.Eq(existingItem => existingItem.Id, item.Id);
            await itemsCollection.ReplaceOneAsync(filter, item);
        }
    }
}