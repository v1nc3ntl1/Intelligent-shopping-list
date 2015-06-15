using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainObject;
using MongoDB.Bson.Serialization;

namespace DataAccess
{
    public class BaseMongoDao
    {
        private static Nullable<bool> _registered; 
        public BaseMongoDao()
        {
            if (!_registered.HasValue || !_registered.Value)
            {
                BsonClassMap.RegisterClassMap<Item>(cm =>
                {
                    cm.MapProperty(p => p.ItemName).SetElementName("ItemName");
                    cm.MapProperty(P => P.Tag).SetElementName("Tag");
                });
                _registered = true;
            }
        }
    }
}
