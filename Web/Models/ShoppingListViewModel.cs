using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Models
{
    public class ShoppingListViewModel
    {
        public string ListName { get; set; }

        public bool IsActive { get; set; }

        public Collection<ItemViewModel> Items { get; set; }
    }
}