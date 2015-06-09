using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using DomainObject;

namespace BusinessLogic
{
    public class DefaultShoppingListCreator : IShoppingListCreator
    {
        private IShoppingListDao _dao;

        public IShoppingListDao Dao
        {
            get { return _dao; }
            set { _dao = value; }
        }
        
        async public Task<bool> CreateShoppingList(ShoppingList shoppingList)
        {
            bool success = await Dao.InsertShoppingList(shoppingList);
            return success;
        }

        async public Task<bool> SaveShoppingList(ShoppingList shoppingList)
        {
            bool success = await Dao.SaveShoppingList(shoppingList);
            return success;
        }
    }
}
