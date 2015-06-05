using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BusinessLogic;
using DomainObject;
using Framework;
using Web.Models;

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

        async public Task<ActionResult> CreateItem()
        {
            CreateItemViewModel model = new CreateItemViewModel();

            IShoppingListService service = SpringResolver.GetObject<IShoppingListService>("ShoppingListServiceImpl");
            var lists = await service.GetShoppingLists();
            if (!lists.IsNullOrEmpty())
            {
                model.AllShoppingList = new Collection<SelectListItem>();
                foreach (var item in lists)
                {
                    model.AllShoppingList.Add(new SelectListItem() {Text = item, Value = item});
                }
                model.AllShoppingList[0].Selected = true;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateItem(CreateItemViewModel model)
        {
            
            return View();
        }

        [HttpPost]
        async public Task<ActionResult> Create(ShoppingList shoppingList)
        {
            IShoppingListCreator creator = SpringResolver.GetObject<IShoppingListCreator>("ShoppingListCreatorImpl");
            bool result = await creator.CreateShoppingList(shoppingList);
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
