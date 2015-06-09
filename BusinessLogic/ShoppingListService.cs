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
    public class ShoppingListService : IShoppingListService
    {
        private IShoppingListDao _dao;

        public IShoppingListDao Dao
        {
            get { return _dao; }
            set { _dao = value; }
        }

        async public Task<Collection<string>> GetShoppingLists()
        {
            var list = await Dao.GetShoppingLists();
            return list.Safely().Select(r => r.ListName).Safely().ToCollection();
        }

        async public Task<ShoppingList> GetShoppingList(string id)
        {
            var list = await Dao.GetShoppingLists(id);
            return list.IsNullOrEmpty() ? null : list[0];
        }

        async public Task<Collection<string>> GetTags()
        {
            var list = await Dao.GetShoppingLists();
            Collection<string> result = null;
            if (!list.IsNullOrEmpty())
            {
                result = new Collection<string>();
                foreach (var item in list)
                {
                    if (!item.Item.IsNullOrEmpty())
                    {
                        foreach (var i in item.Item)
                        {
                            if (!result.Contains(i.Tag))
                                result.Add(i.Tag);
                        }
                    }
                }
            }
            return result;
        }
    }
}
