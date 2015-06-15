using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using DomainObject;

namespace Web.Models
{
    public class PromotionViewModel : Promotion
    {
        new public string Id { get; set; }

        public Collection<string> Tags { get; set; }

        public Collection<string> Items { get; set; } 
    }
}