using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainObject
{
    public class Item
    {
        private string _id;

        public string ObjectId
        {
            get { return  _id; }
            set {  _id = value; }
        }

        private string _itemName;

        public string ItemName
        {
            get { return _itemName; }
            set { _itemName = value; }
        }

        private string[] _tag;

        public string[] Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }
        
    }
}
