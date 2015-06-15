﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainObject;

namespace BusinessLogic
{
    public interface IPromotionExtractor
    {
        Task<Promotion> GetPromotion(string id);

        Task<Collection<Promotion>> GetPromotion();

        Task<Collection<Promotion>> GetActivePromotion();
    }
}
