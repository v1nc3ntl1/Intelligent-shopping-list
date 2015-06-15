using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainObject;

namespace BusinessLogic
{
    public interface IPromotionCreator
    {
        Task<bool> CreatePromotion(Promotion promotion);

        Task<bool> SavePromotion(Promotion promotion);
    }
}
