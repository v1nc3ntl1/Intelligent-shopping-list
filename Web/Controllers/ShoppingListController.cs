using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BusinessLogic;
using DomainObject;
using Framework;

namespace Web.Controllers
{
    public class ShoppingListController : Controller
    {
        //
        // GET: /ShoppingList/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ShoppingList shoppingList)
        {
            IShoppingListCreator creator = SpringResolver.GetObject<IShoppingListCreator>("ShoppingListCreatorImpl");
            creator.CreateShoppingList(new ShoppingList());
            return View();
        }

        async public Task<ActionResult> ListShoppingListItem()
        {
            IShoppingListItemExtractor extractor =
                SpringResolver.GetObject<IShoppingListItemExtractor>("ShoppingListItemExtractorImpl");
            var model = extractor.GetActiveItem();
            return View(await model);
        }
    }
}
