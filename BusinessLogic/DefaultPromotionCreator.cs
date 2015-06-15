using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using DomainObject;

namespace BusinessLogic
{
    public class DefaultPromotionCreator : IPromotionCreator
    {
        public IPromotionDao Dao { get; set; }

        async public Task<bool> CreatePromotion(Promotion promotion)
        {
            return await Dao.SavePromotion(promotion);
        }

        async public Task<bool> SavePromotion(Promotion promotion)
        {
            return await Dao.SavePromotion(promotion);
        }
    }
}
