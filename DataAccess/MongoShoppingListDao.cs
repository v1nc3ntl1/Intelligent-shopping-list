using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainObject;
using Framework;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;

namespace DataAccess
{
    public class MongoShoppingListDao : IShoppingListDao
    {
        private IConnectionManager _connectionManager;

        private string _dbConnectionStringConfigKey;

        public string DBConnectionStringConfigKey
        {
            get { return _dbConnectionStringConfigKey; }
            set { _dbConnectionStringConfigKey = value; }
        }

        public MongoShoppingListDao(IConnectionManager connectionManager) : this()
        {
            _connectionManager = connectionManager;
        }

        public MongoShoppingListDao()
        {
            BsonClassMap.RegisterClassMap<ShoppingList>(cm =>
            {
                cm.MapProperty(c => c.ListName);
                cm.MapProperty(c => c.IsActive);
                cm.SetIdMember(cm.GetMemberMap(c => c.Id));
            });
            BsonClassMap.RegisterClassMap<Item>(cm =>
            {
                cm.MapProperty(p => p.ItemName);
                cm.MapProperty(P => P.Tag);
            });
        }

        async public Task<bool> InsertShoppingList(ShoppingList shoppingList)
        {
            bool success = true;
            try
            {
                var client = new MongoClient(_connectionManager.GetConnectionString(DBConnectionStringConfigKey));
                var database = client.GetDatabase("Intelligent_Shopping_List");
                var collection = database.GetCollection<BsonDocument>("ShoppingList");
                var document = new BsonDocument 
                { 
                    { "ListName", "My List 3"},
                    {
                        "ShoppingListItems", new BsonArray() 
                        {
                            new BsonDocument()
                        {
                            {"ItemName", "mouse"},
                            {"Tag", "mouse"}
                        }
                        }
                    },
                    {"IsActive", "true"}};
                await collection.InsertOneAsync(document);
            }
            catch (Exception)
            {
                success = false;
                //TODO: Log error
            }
            return success;
        }

        async public Task<Collection<ShoppingList>> GetShoppingLists()
        {
            var client = new MongoClient(_connectionManager.GetConnectionString(DBConnectionStringConfigKey));
            var database = client.GetDatabase("Intelligent_Shopping_List");
            var collection = database.GetCollection<BsonDocument>("ShoppingList");
           
            var result = await collection.Find(new BsonDocument()).ToListAsync();
            var jsonresult = result.ToJson();
            var final = new Collection<ShoppingList>();
            if (result != null && result.Count > 0)
            {
                ShoppingList tempHolder;
                BsonArray rawItems;

                foreach (var item in result)
                {
                    tempHolder = BsonSerializer.Deserialize<ShoppingList>(item);
                    tempHolder.Id = item["_id"].AsObjectId;
                    rawItems = item["ShoppingListItems"].AsBsonArray;
                    if (rawItems != null)
                    {
                        tempHolder.Item = new Collection<Item>();
                        foreach (var rawItem in rawItems)
                        {
                            tempHolder.Item.Add(BsonSerializer.Deserialize<Item>(rawItem.AsBsonDocument));
                        }
                    }
                    final.Add(tempHolder);
                }
            }
            return final;
        }

        public Collection<ShoppingList> GetShoppingLists(bool isActive)
        {
            return null;
        }
    }
}
