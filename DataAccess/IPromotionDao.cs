using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainObject;

namespace DataAccess
{
    public interface IPromotionDao
    {
        Task<bool> SavePromotion(Promotion promotion);

        Task<Collection<Promotion>> GetPromotion(string id = "");

        Task<Collection<Promotion>> GetPromotion(DateTime effectiveDateTime);

        Task<Collection<Promotion>> GetPromotion(Collection<string> tag);

        Task<Collection<string>> GetBrands();

        Task<Collection<string>> GetTags();
    }
}
