using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using DomainObject;
using Framework;

namespace BusinessLogic
{
    public class DefaultShoppingListItemExtractor : IShoppingListItemExtractor
    {
        private IShoppingListDao _dao;

        public IShoppingListDao Dao
        {
            get { return _dao; }
            set { _dao = value; }
        }

        async public Task<Collection<Item>> GetActiveItem()
        {
            Collection<ShoppingList> lists = await Dao.GetShoppingLists();
            
            Collection<Item> listItems = new Collection<Item>();
            foreach (ShoppingList list in lists.Where(l => l.IsActive))
            {
                if (!list.Item.IsNullOrEmpty())
                {
                    foreach (var item in list.Item)
                    {
                        listItems.Add(item);
                    }
                }
            }
            return listItems;
        }
    }
}
