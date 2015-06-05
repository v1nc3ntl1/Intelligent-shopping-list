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
        public string ListName { get; set; }

        public Collection<SelectListItem> AllShoppingList { get; set; }

        public string ItemName { get; set; }

        public string Tag { get; set; }

        public IEnumerable<SelectListItem> AllTags { get; set; }
    }
}