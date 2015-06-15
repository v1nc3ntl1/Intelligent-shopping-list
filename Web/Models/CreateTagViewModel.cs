using System.Collections.ObjectModel;
using System.Web.Mvc;

namespace Web.Models
{
    public class CreateTagViewModel
    {
        public Collection<SelectListItem> AllTags { get; set; }

        public string Tag { get; set; }

        public string SelectedTag { get; set; }

        public string EnteredTag { get; set; }

        public string PromotionId { get; set; } 
    }
}