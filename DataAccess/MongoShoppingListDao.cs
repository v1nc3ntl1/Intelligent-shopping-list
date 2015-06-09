using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainObject;
using Framework;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

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
            : this()
        {
            _connectionManager = connectionManager;
        }

        public MongoShoppingListDao()
        {
            BsonClassMap.RegisterClassMap<ShoppingList>(cm =>
            {
                cm.MapProperty(c => c.ListName).SetElementName("ListName");
                cm.MapProperty(c => c.IsActive).SetElementName("IsActive");
                cm.MapProperty(c => c.Item).SetElementName("ShoppingListItems");
                cm.SetIdMember(cm.GetMemberMap(c => c.Id));
            });
            BsonClassMap.RegisterClassMap<Item>(cm =>
            {
                cm.MapProperty(p => p.ItemName).SetElementName("ItemName");
                cm.MapProperty(P => P.Tag).SetElementName("Tag");
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
                //var document = new BsonDocument 
                //{ 
                //    { "ListName", "My List 3"},
                //    {
                //        "ShoppingListItems", new BsonArray() 
                //        {
                //            new BsonDocument()
                //        {
                //            {"ItemName", "mouse"},
                //            {"Tag", "mouse"}
                //        }
                //        }
                //    },
                //    {"IsActive", "true"}};
                shoppingList.Id = ObjectId.GenerateNewId();

                BsonDocument dbBsonDocument = shoppingList.ToBsonDocument(typeof (ShoppingList));

                await collection.InsertOneAsync(dbBsonDocument);
            }
            catch (Exception ex)
            {
                success = false;
                //TODO: Log error
            }
            return success;
        }

        async public Task<bool> SaveShoppingList(ShoppingList shoppingList)
        {
            bool success = true;
            if (shoppingList.Id == ObjectId.Empty)
                return await InsertShoppingList(shoppingList);

            try
            {
                var client = new MongoClient(_connectionManager.GetConnectionString(DBConnectionStringConfigKey));
                var database = client.GetDatabase("Intelligent_Shopping_List");
                var collection = database.GetCollection<BsonDocument>("ShoppingList");

                var filter = new BsonDocument("_id", shoppingList.Id);
                await collection.ReplaceOneAsync(filter, shoppingList.ToBsonDocument(typeof(ShoppingList)));
            }
            catch (Exception)
            {
                success = false;
                //TODO: Log error
            }
            return success;
        }

        async public Task<Collection<ShoppingList>> GetShoppingLists(string id = "")
        {
            var client = new MongoClient(_connectionManager.GetConnectionString(DBConnectionStringConfigKey));
            var database = client.GetDatabase("Intelligent_Shopping_List");
            var collection = database.GetCollection<BsonDocument>("ShoppingList");
            var filter = string.IsNullOrEmpty(id) ? new BsonDocument() : new BsonDocument {{"_id", ObjectId.Parse(id)}};


            var result = await collection.Find(filter).ToListAsync();
            var jsonresult = result.ToJson();
            var final = new Collection<ShoppingList>();
            if (result != null && result.Count > 0)
            {
                ShoppingList tempHolder;
                BsonArray rawItems;
                BsonElement tempBsonElement;

                foreach (var item in result)
                {
                    tempHolder = BsonSerializer.Deserialize<ShoppingList>(item);
                    tempHolder.Id = item["_id"].AsObjectId;
                    rawItems = item.TryGetElement("ShoppingListItems", out tempBsonElement) ? item["ShoppingListItems"].AsBsonArray : null;
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
