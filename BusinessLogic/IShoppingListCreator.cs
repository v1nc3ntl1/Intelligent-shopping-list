using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainObject;

namespace BusinessLogic
{
    public interface IShoppingListCreator
    {
        Task<bool> CreateShoppingList(ShoppingList shoppingList);

        Task<bool> SaveShoppingList(ShoppingList shoppingList);
    }
}
