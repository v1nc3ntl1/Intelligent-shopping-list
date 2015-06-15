using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using BusinessLogic;
using DomainObject;
using Framework;
using MongoDB.Bson;
using Web.Models;

namespace Web.Controllers
{
    public class ShoppingListController : Controller
    {
        private IShoppingListService _shoppingListService;

        public IShoppingListService ShoppingListService
        {
            get { return _shoppingListService = _shoppingListService ?? SpringResolver.GetObject<IShoppingListService>("ShoppingListServiceImpl");; }
        }

        private IShoppingListCreator _shoppingListCreator;

        public IShoppingListCreator ShoppingListCreator
        {
            get { return _shoppingListCreator = _shoppingListCreator ?? SpringResolver.GetObject<IShoppingListCreator>("ShoppingListCreatorImpl"); }
        }
        
        
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

        async public Task<ActionResult> CreateItem(string id)
        {
            CreateItemViewModel model = new CreateItemViewModel();

            ShoppingList result = null;
            if (!string.IsNullOrEmpty(id))
            {
                result = await ShoppingListService.GetShoppingList(id);
                if (result != null) model.ShoppingListId = result.Id.ToString();
            }

            var lists = await ShoppingListService.GetShoppingLists();
            if (!lists.IsNullOrEmpty())
            {
                model.AllShoppingList = new Collection<SelectListItem>();
                foreach (var item in lists)
                {
                    if (result == null || !string.Equals(result.ListName, item))
                        model.AllShoppingList.Add(new SelectListItem() { Text = item, Value = item });
                    else
                    {
                        model.AllShoppingList.Add(new SelectListItem() { Text = item, Value = item , Selected = true});
                    }
                }
            }

            var allTags = await ShoppingListService.GetTags();
            if (!allTags.IsNullOrEmpty())
            {
                model.AllTags = new Collection<SelectListItem>();
                foreach (var item in allTags)
                {
                    if (result == null || !result.Item.Safely().Any(i => string.Equals(i.Tag, item)))
                        model.AllTags.Add(new SelectListItem() { Text = item, Value = item });
                    else
                    {
                        model.AllTags.Add(new SelectListItem() { Text = item, Value = item, Selected = true});
                    }
                }
            }
            
            return View(model);
        }

        [HttpPost]
        async public Task<ActionResult> CreateItem(CreateItemViewModel model)
        {
            ShoppingList shoppingList = new ShoppingList();

            //Initialization
            shoppingList.IsActive = true;

            if (!string.IsNullOrEmpty(model.ShoppingListId))
            {
                shoppingList.Id = ObjectId.Parse(model.ShoppingListId);
                var existing = await ShoppingListService.GetShoppingList(model.ShoppingListId);
                shoppingList.Item = existing.Item;
                shoppingList.IsActive = existing.IsActive;
            }
            else if (!string.IsNullOrEmpty(model.SelectedListName))
            {
                //If user selected an existing list name
                var existing = await ShoppingListService.GetShoppingListByName(model.SelectedListName);
                if (!existing.IsNullOrEmpty())
                {
                    var firstMatch = existing.FirstOrDefault();
                    shoppingList.Id = firstMatch.Id;
                    shoppingList.Item = firstMatch.Item;
                    shoppingList.IsActive = firstMatch.IsActive;
                }
            }
                
            shoppingList.ListName = model.ListName;
            if (shoppingList.Item == null) shoppingList.Item = new Collection<Item>();
            shoppingList.Item.Add(new Item(){ItemName = model.ItemName, Tag = model.Tag});

            var result = await this.ShoppingListCreator.SaveShoppingList(shoppingList);
            if (!result)
            {
                TempData["error"] = "Operation save failed";
                return await CreateItem(model.ShoppingListId);
            }
            return RedirectToAction("ListShoppingListItem");
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

        #region

        #endregion
    }
}
