using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
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
    }
}
