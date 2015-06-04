using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainObject
{
    public class Promotion
    {
        private string _id;

        public string ObjectId
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

        private Item[] _items;

        public Item[] PromotionItems
        {
            get { return _items; }
            set { _items = value; }
        }

        private DateTime _effectiveDateTime;

        public DateTime EffectiveDateTime
        {
            get { return _effectiveDateTime; }
            set { _effectiveDateTime = value; }
        }

        private DateTime _effectiveEndDateTime;

        public DateTime EffectiveEndDateTime
        {
            get { return _effectiveEndDateTime; }
            set { _effectiveEndDateTime = value; }
        }

        private string[] _brands;

        public string[] Brands
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
        
    }
}
