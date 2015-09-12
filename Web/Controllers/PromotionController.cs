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
using MongoDB.Bson;
using Web.Models;

namespace Web.Controllers
{
    public class PromotionController : Controller
    {

        /// <summary>
        /// The _shopping list service
        /// </summary>
        private IShoppingListService _shoppingListService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PromotionController"/> class.
        /// </summary>
        public PromotionController()
        {
            _shoppingListService = SpringResolver.GetObject<IShoppingListService>("ShoppingListServiceImpl");
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        async public Task<ActionResult> Index()
        {
            var extractor = SpringResolver.GetObject<IPromotionExtractor>("PromotionExtractorImpl");
            var allPromotion = await extractor.GetPromotion();

            Collection<PromotionViewModel> model = model = new Collection<PromotionViewModel>();
            if (!allPromotion.IsNullOrEmpty())
            {
                foreach (var promotion in allPromotion)
                {
                    model.Add(new PromotionViewModel()
                    {
                        Id = promotion.Id.ToString(),
                        EffectiveDateTime = promotion.EffectiveDateTime,
                        EffectiveEndDateTime = promotion.EffectiveEndDateTime,
                        Brands = promotion.Brands.Safely().Distinct().ToCollection(),
                        Location = promotion.Location,
                        PromotionName = promotion.PromotionName,
                        Items = promotion.PromotionItems.Safely().Select(i => i.ItemName).ToCollection(),
                        Tags = promotion.PromotionItems.Safely().Select(i => i.Tag).Distinct().ToCollection()
                    });
                }
            }
            return View(model);
        }

        /// <summary>
        /// Creates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        async public Task<ActionResult> Create(string id)
        {
            CreatePromotionViewModel model = new CreatePromotionViewModel();
            if (!string.IsNullOrEmpty(id))
            {
                var extractor = SpringResolver.GetObject<IPromotionExtractor>("PromotionExtractorImpl");
                var promotion = await extractor.GetPromotion(id);
                if (promotion != null)
                {
                    model.Id = promotion.Id.ToString();
                    model.Brands = promotion.Brands;
                    model.EffectiveDateTime = promotion.EffectiveDateTime;
                    model.EffectiveEndDateTime = promotion.EffectiveEndDateTime;
                    model.Name = promotion.PromotionName;
                    model.Location = promotion.Location;
                    model.Tags = promotion.PromotionItems.Safely().Select(p => p.Tag).Safely().ToCollection<string>();
                    model.Brands = promotion.Brands;
                }
            }
            
            return View(model);
        }

        /// <summary>
        /// Creates the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        async public Task<ActionResult> Create(CreatePromotionViewModel model)
        {
            var creator = SpringResolver.GetObject<IPromotionCreator>("PromotionCreatorImpl");
            Promotion promotion = new Promotion()
            {
                PromotionName = model.Name,
                Location = model.Location,
                EffectiveDateTime = model.EffectiveDateTime,
                EffectiveEndDateTime = model.EffectiveEndDateTime,
            };
            if (!string.IsNullOrEmpty(model.Id))
            {
                promotion.Id = ObjectId.Parse(model.Id);
                var extractor = SpringResolver.GetObject<IPromotionExtractor>("PromotionExtractorImpl");
                var existing = await extractor.GetPromotion(model.Id);
                if (existing != null)
                {
                    promotion.Brands = existing.Brands;
                    promotion.PromotionItems = existing.PromotionItems;
                }
            }

            var result = await creator.SavePromotion(promotion);
            if (!result)
            {
                TempData["error"] = "Error updating promotion";
                return await Create(model.Id);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Creates the brand.
        /// </summary>
        /// <param name="promotionId">The promotion identifier.</param>
        /// <returns></returns>
        async public Task<ActionResult> CreateBrand(string promotionId)
        {
            var allBrands = await this._shoppingListService.GetBrands();
            CreateBrandViewModel model = new CreateBrandViewModel();
            model.PromotionId = promotionId;
            if (!allBrands.IsNullOrEmpty())
            {
                model.AllBrand = new Collection<SelectListItem>();
                foreach (var brand in allBrands)
                    model.AllBrand.Add(new SelectListItem()
                    {
                        Text = brand,
                        Value = brand
                    });
            }
            
            return View(model);
        }

        /// <summary>
        /// Creates the brand.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        async public Task<ActionResult> CreateBrand(CreateBrandViewModel model)
        {
            if (string.IsNullOrEmpty(model.PromotionId))
            {
                TempData["error"] = "Invalid state encountered!";
                return await CreateBrand(string.Empty);
            }
            var extractor = SpringResolver.GetObject<IPromotionExtractor>("PromotionExtractorImpl");
            var promotion = await extractor.GetPromotion(model.PromotionId);

            if (promotion == null)
            {
                TempData["error"] = "Promotion not found. Unable to save!";
                return await CreateBrand(string.Empty);
            }
            if (promotion.Brands == null)
                promotion.Brands = new Collection<string>();
            promotion.Brands.Add(model.Brand);
            var creator = SpringResolver.GetObject<IPromotionCreator>("PromotionCreatorImpl");
            var result = await creator.SavePromotion(promotion);
            if (result)
                return RedirectToAction("Create", "Promotion", new {id = promotion.Id.ToString()});
            TempData["error"] = "Unable to save.";
            return await CreateBrand(promotion.Id.ToString());
        }

        /// <summary>
        /// Creates the tag.
        /// </summary>
        /// <param name="promotionId">The promotion identifier.</param>
        /// <returns></returns>
        async public Task<ActionResult> CreateTag(string promotionId)
        {
            var allTag = await this._shoppingListService.GetTags();
            CreateTagViewModel model = new CreateTagViewModel
            {
                PromotionId = promotionId
            };
            if (!allTag.IsNullOrEmpty())
            {
                model.AllTags = new Collection<SelectListItem>();
                foreach (var tag in allTag)
                    model.AllTags.Add(new SelectListItem()
                    {
                        Text = tag,
                        Value = tag
                    });
            }
            
            return View(model);
        }

        /// <summary>
        /// Creates the tag.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        async public Task<ActionResult> CreateTag(CreateTagViewModel model)
        {
            if (string.IsNullOrEmpty(model.PromotionId))
            {
                TempData["error"] = "Invalid state encountered!";
                return await CreateTag(string.Empty);
            }
            var extractor = SpringResolver.GetObject<IPromotionExtractor>("PromotionExtractorImpl");
            var promotion = await extractor.GetPromotion(model.PromotionId);

            if (promotion == null)
            {
                TempData["error"] = "Promotion not found. Unable to save!";
                return await CreateTag(string.Empty);
            }
            if (promotion.PromotionItems == null)
                promotion.PromotionItems = new Collection<Item>();
            promotion.PromotionItems.Add(new Item {Tag = model.Tag, ItemName = model.Tag});
            var creator = SpringResolver.GetObject<IPromotionCreator>("PromotionCreatorImpl");
            var result = await creator.SavePromotion(promotion);
            if (result)
                return RedirectToAction("Create", "Promotion", new { id = promotion.Id.ToString() });
            TempData["error"] = "Unable to save.";
            return await CreateTag(promotion.Id.ToString());
        }
    }
}
