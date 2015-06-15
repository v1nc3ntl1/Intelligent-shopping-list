using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DataAccess;
using DomainObject;
using Framework;

namespace BusinessLogic
{
    public class DefaultPromotionExtractor : IPromotionExtractor
    {
        public IPromotionDao Dao { get; set; }

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

        public Task<Collection<Promotion>> GetActivePromotion()
        {
            return null;
        }
    }
}