using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Models
{
    public class CreateBrandViewModel
    {
        public Collection<SelectListItem> AllBrand { get; set; }

        public string Brand { get; set; }

        public string SelectedBrand { get; set; }

        public string EnteredBrand { get; set; }

        public string PromotionId { get; set; }
    }
}