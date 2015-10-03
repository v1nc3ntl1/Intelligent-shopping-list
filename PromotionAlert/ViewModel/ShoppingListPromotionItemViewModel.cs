using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromotionAlert.ViewModel
{
  public class ShoppingListPromotionItemViewModel
  {
    public string PromotionName { get; set; }

    public string Description { get; set; }

    public DateTime EffectiveDateTime { get; set; }

    public string Brands { get; set; }

    public string ListTags { get; set; }

    public string ListItems { get; set; }

    public string PromotionTags { get; set; }

    public string PromotionItems { get; set; }

    public string Link { get; set; }

    public string Html { get; set; }
  }
}