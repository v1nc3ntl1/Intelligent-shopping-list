using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainObject;
using Framework;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace DataAccess
{
    public class MongoPromotionDao : BaseMongoDao, IPromotionDao
    {
        private IConnectionManager _connectionManager;

        private string _promotionTableName;

        public string PromotionTableName
        {
            get { return _promotionTableName; }
            set { _promotionTableName = value; }
        }
        

        private string _promotionDatabaseName;

        public string PromotionDatabaseName
        {
            get { return _promotionDatabaseName; }
            set { _promotionDatabaseName = value; }
        }

        private IMongoDatabase _promotionDatabase;

        public MongoPromotionDao(IConnectionManager connectionManager, string dbConnectionStringConfigKey, string databaseName)
            : this()
        {
            _connectionManager = connectionManager;
            var client = new MongoClient(_connectionManager.GetConnectionString(dbConnectionStringConfigKey));
            _promotionDatabase = client.GetDatabase(databaseName);
        }

        public MongoPromotionDao()
        {
            BsonClassMap.RegisterClassMap<Promotion>(cm =>
            {
                cm.MapProperty(c => c.PromotionName).SetElementName("PromotionName");
                cm.MapProperty(c => c.Description).SetElementName("Description");
                cm.MapProperty(c => c.Link).SetElementName("Link");
                cm.MapProperty(c => c.Html).SetElementName("Html");
                cm.MapProperty(c => c.EffectiveDateTime).SetElementName("EffectiveDateTime");
                cm.MapProperty(c => c.EffectiveEndDateTime).SetElementName("EffectiveEndDateTime");
                cm.MapProperty(c => c.Location).SetElementName("Location");
                cm.MapProperty(c => c.PromotionItems).SetElementName("PromotionItems");
                cm.MapProperty(c => c.Brands).SetElementName("Brands");
                cm.SetIdMember(cm.GetMemberMap(c => c.Id));
            });
        }

        async public Task<bool> InsertPromotion(Promotion promotion)
        {
            bool success = true;
            try
            {
                var collection = _promotionDatabase.GetCollection<BsonDocument>(PromotionTableName);

                promotion.Id = ObjectId.GenerateNewId();

                BsonDocument dbBsonDocument = promotion.ToBsonDocument(typeof(Promotion));

                await collection.InsertOneAsync(dbBsonDocument);
            }
            catch (Exception ex)
            {
                success = false;
                //TODO: Log error
            }
            return success;
        }

        async public Task<bool> SavePromotion(Promotion promotion)
        {
            bool success = true;
            if (promotion.Id == ObjectId.Empty)
                return await InsertPromotion(promotion);
            try
            {
                var collection = _promotionDatabase.GetCollection<BsonDocument>(PromotionTableName);

                var filter = new BsonDocument("_id", promotion.Id);
                await collection.ReplaceOneAsync(filter, promotion.ToBsonDocument(typeof(Promotion)));
            }
            catch (Exception)
            {
                success = false;
                //TODO: Log error
            }
            return success;
        }

        async public Task<Collection<Promotion>> GetPromotion(string id = "")
        {
            var collection = _promotionDatabase.GetCollection<BsonDocument>(PromotionTableName);

            var filter = string.IsNullOrEmpty(id) ? new BsonDocument() : new BsonDocument { { "_id", ObjectId.Parse(id) } };

            var result = await collection.Find(filter).ToListAsync();
            return convertPromotionToCollection(result);
        }

        async public Task<Collection<Promotion>> GetPromotion(DateTime effectiveDateTime)
        {
          var collection = _promotionDatabase.GetCollection<BsonDocument>(PromotionTableName);

          var builder = Builders<BsonDocument>.Filter;
          var filter = builder.Gt("EffectiveDateTime", effectiveDateTime);

          var result = await collection.Find(filter).ToListAsync();
          return convertPromotionToCollection(result);
        }

        async public Task<Collection<Promotion>> GetPromotion(Collection<string> tag)
        {
            var collection = _promotionDatabase.GetCollection<BsonDocument>(PromotionTableName);

            //var filter = new BsonDocument { { "PromotionItems.Tag", tag } };

            var result = await collection.Find(new BsonDocument()).ToListAsync();
            return convertPromotionToCollection(result);
        }

        async public Task<Collection<string>> GetBrands()
        {
            var collection = _promotionDatabase.GetCollection<Promotion>(PromotionTableName);
            var brands = await collection.Find(new BsonDocument()).Project(p => p.Brands).ToListAsync();
            //var brands = await collection.Aggregate(new AggregateOptions()).Project(p => p.Brands).ToListAsync();
            Collection<string> final = new Collection<string>();
            foreach (Collection<string> brand in brands)
            {
                if (!brand.IsNullOrEmpty())
                {
                    foreach (string innerBrand in brand)
                        final.Add(innerBrand);
                }
            }
            return final.Distinct().ToCollection();
        }

        async public Task<Collection<string>> GetTags()
        {
            var collection = _promotionDatabase.GetCollection<Promotion>(PromotionTableName);
            var promotionItems = await collection.Find(new BsonDocument()).Project(p => p.PromotionItems).ToListAsync();
            
            Collection<string> final = new Collection<string>();
            foreach (Collection<Item> item in promotionItems)
            {
                if (!item.IsNullOrEmpty())
                {
                    foreach (Item innerItem in item)
                        final.Add(innerItem.Tag);
                }
            }
            return final.Distinct().ToCollection();
        }

        private Collection<Promotion> convertPromotionToCollection(List<BsonDocument> rawList)
        {
            var jsonresult = rawList.ToJson();
            var final = new Collection<Promotion>();
            if (rawList != null && rawList.Count > 0)
            {
                Promotion tempHolder;
                BsonArray rawItems;
                BsonElement tempBsonElement;

                foreach (var item in rawList)
                {
                    tempHolder = BsonSerializer.Deserialize<Promotion>(item);
                    tempHolder.Id = item["_id"].AsObjectId;
                    rawItems = item.TryGetElement("PromotionItems", out tempBsonElement) && !(tempBsonElement.Value is BsonNull) ? item["PromotionItems"].AsBsonArray : null;
                    if (rawItems != null)
                    {
                        tempHolder.PromotionItems = new Collection<Item>();
                        foreach (var rawItem in rawItems)
                        {
                            tempHolder.PromotionItems.Add(BsonSerializer.Deserialize<Item>(rawItem.AsBsonDocument));
                        }
                    }
                    final.Add(tempHolder);
                }
            }
            return final;
        }
    }
}
