using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace DomainObject
{
    public class ShoppingList : IEquatable<ShoppingList>, IComparable<ShoppingList>
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

        public override bool Equals(object obj)
        {
            ShoppingList castOList = obj as ShoppingList;
            if (castOList == null)
                return false;
            return Equals(castOList);
        }

        public override int GetHashCode()
        {
            return this.Id.ToString().GetHashCode();
        }

        public bool Equals(ShoppingList shoppingList)
        {
            if (shoppingList == null)
                return false;
            return string.Equals(this.Id, shoppingList.Id);
        }

        //public static bool operator ==(ShoppingList list1, ShoppingList list2)
        //{
        //    if (list1 == null || list2 == null)
        //        return object.Equals(list1, list2);
        //    return list1.Equals(list2);
        //}

        //public static bool operator !=(ShoppingList list1, ShoppingList list2)
        //{
        //    if (list1 == null || list2 == null)
        //        return !object.Equals(list1, list2);
        //    return !list1.Equals(list2);
        //}

        public int CompareTo(ShoppingList shoppingList)
        {
             if (shoppingList == null) return 1;

            return this.ListName.CompareTo(shoppingList.ListName);
        }
    }
}
