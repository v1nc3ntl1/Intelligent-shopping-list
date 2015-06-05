using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public interface IShoppingListService
    {
        Task<Collection<string>> GetShoppingLists();
    }
}
