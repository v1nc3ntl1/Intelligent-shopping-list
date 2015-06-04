using DomainObject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IShoppingListDao
    {
        Task<bool> InsertShoppingList(ShoppingList shoppingList);

        Task<Collection<ShoppingList>> GetShoppingLists();

        Collection<ShoppingList> GetShoppingLists(bool isActive);
    }
}
