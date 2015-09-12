using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace DomainObject
{
    public class Promotion
    {
        private ObjectId _id;

        public ObjectId Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;

        public string PromotionName
        {
            get { return _name; }
            set { _name = value; }
        }

        private Collection<Item> _items;

        public Collection<Item> PromotionItems
        {
            get { return _items; }
            set { _items = value; }
        }

        private DateTime _effectiveDateTime = DateTime.MinValue;

        public DateTime EffectiveDateTime
        {
            get { return _effectiveDateTime; }
            set { _effectiveDateTime = value; }
        }

        private DateTime _effectiveEndDateTime = DateTime.MaxValue;

        public DateTime EffectiveEndDateTime
        {
            get { return _effectiveEndDateTime; }
            set { _effectiveEndDateTime = value; }
        }

        private Collection<string> _brands;

        public Collection<string> Brands
        {
            get { return _brands; }
            set { _brands = value; }
        }

        private string _location;

        public string Location
        {
            get { return _location; }
            set { _location = value; }
        }

        private string _description;

        public string Description
        {
          get { return _description; }
          set { _description = value; }
        }
      
    }
}
