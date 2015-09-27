using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DataAccess;
using DomainObject;
using Framework;

namespace BusinessLogic
{
  using System;

  public class DefaultPromotionExtractor : IPromotionExtractor
    {
        public IPromotionDao Dao { get; set; }

        /// <summary>
        /// The number of days since promotion being publish to be consider as active promotion
        /// </summary>
        private int NumberOfDaysSincePublishAsActive { get; set; }

        async public Task<Promotion> GetPromotion(string id)
        {
            var result = await this.Dao.GetPromotion(id);
            if (!result.IsNullOrEmpty())
                return result[0];
            return null;
        }
      
        async public Task<Collection<Promotion>> GetPromotion()
        {
            return await this.Dao.GetPromotion();
        }

        async public Task<Collection<Promotion>> GetActivePromotion()
        {
          var result = await this.Dao.GetPromotion(DateTime.Now.AddDays(-NumberOfDaysSincePublishAsActive));
          if (!result.IsNullOrEmpty())
            return result;
          return null;
        }

    }
}