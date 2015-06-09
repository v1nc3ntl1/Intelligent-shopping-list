using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Models
{
    public class CreateItemViewModel
    {
        public string ShoppingListId { get; set; }

        public string ListName { get; set; }

        public string SelectedListName { get; set; }

        public string EnteredListName { get; set; }

        public Collection<SelectListItem> AllShoppingList { get; set; }

        public string ItemName { get; set; }

        public string SelectedTag { get; set; }

        public string EnteredTag { get; set; }

        public string Tag { get; set; }

        public Collection<SelectListItem> AllTags { get; set; }
    }
}