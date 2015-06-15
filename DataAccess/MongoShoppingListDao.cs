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
    public class MongoShoppingListDao : BaseMongoDao, IShoppingListDao
    {
        private IConnectionManager _connectionManager;

        private string _shoppingListTableName;

        public string ShoppingListTableName
        {
            get { return _shoppingListTableName; }
            set { _shoppingListTableName = value; }
        }
      
        private IMongoDatabase _shoppingListDatabase;

        public MongoShoppingListDao(IConnectionManager connectionManager, string dbConnectionStringConfigKey, string databaseName)
            : this()
        {
            _connectionManager = connectionManager;
            var client = new MongoClient(_connectionManager.GetConnectionString(dbConnectionStringConfigKey));
            _shoppingListDatabase = client.GetDatabase(databaseName);
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
        }

        async public Task<bool> InsertShoppingList(ShoppingList shoppingList)
        {
            bool success = true;
            try
            {
                var collection = _shoppingListDatabase.GetCollection<BsonDocument>(ShoppingListTableName);

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
                var collection = _shoppingListDatabase.GetCollection<BsonDocument>(ShoppingListTableName);

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
            var collection = _shoppingListDatabase.GetCollection<BsonDocument>(ShoppingListTableName);
            
            var filter = string.IsNullOrEmpty(id) ? new BsonDocument() : new BsonDocument {{"_id", ObjectId.Parse(id)}};

            var result = await collection.Find(filter).ToListAsync();
            return convertShoppingListToCollection(result);
        }

        async public Task<Collection<ShoppingList>> GetShoppingListsByName(string listName)
        {
            var collection = _shoppingListDatabase.GetCollection<BsonDocument>(ShoppingListTableName);
            
            var filter = new BsonDocument { { "ListName", listName } };

            var result = await collection.Find(filter).ToListAsync();
            return convertShoppingListToCollection(result);
        }

        private Collection<ShoppingList> convertShoppingListToCollection(List<BsonDocument> rawList)
        {
            var jsonresult = rawList.ToJson();
            var final = new Collection<ShoppingList>();
            if (rawList != null && rawList.Count > 0)
            {
                ShoppingList tempHolder;
                BsonArray rawItems;
                BsonElement tempBsonElement;

                foreach (var item in rawList)
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

        /// <summary>
        /// This piece of code is not tested yet.
        /// </summary>
        /// <param name="collectionName"></param>
        async private void CreateCollection(string collectionName)
        {
            await _shoppingListDatabase.CreateCollectionAsync(collectionName,
                new CreateCollectionOptions() {MaxSize = 10000, Capped = true});
        }

        public Collection<ShoppingList> GetShoppingLists(bool isActive)
        {
            return null;
        }
    }
}
