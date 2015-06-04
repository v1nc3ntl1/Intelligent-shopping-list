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

        public MongoShoppingListDao(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
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
            BsonClassMap.RegisterClassMap<ShoppingList>(cm =>
            {
                cm.MapProperty(p => p.ListName);
                cm.MapProperty(p => p.Item);
                cm.MapProperty(p => p.IsActive);
            });

            var client = new MongoClient(_connectionManager.GetConnectionString(DBConnectionStringConfigKey));
            var database = client.GetDatabase("Intelligent_Shopping_List");
            var collection = database.GetCollection<BsonDocument>("ShoppingList");
           
            var result = await collection.Find(new BsonDocument()).ToListAsync();
            var jsonresult = result.ToJson();
            var final = new Collection<ShoppingList>();
            if (result != null && result.Count > 0)
            {
                foreach (var item in result)
                {
                    final.Add(BsonSerializer.Deserialize<ShoppingList>(item));
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
