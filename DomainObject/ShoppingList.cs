using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace DomainObject
{
    public class ShoppingList
    {
        private ObjectId _id;

        public ObjectId Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _listName;

        public string ListName
        {
            get { return _listName; }
            set { _listName = value; }
        }

        private Collection<Item> _item;

        public Collection<Item> Item
        {
            get { return _item; }
            set { _item = value; }
        }

        private bool _isActive;

        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }
        
    }
}
