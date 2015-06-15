using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Models
{
    public class CreatePromotionViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        [UIHint("EditDate")]
        public DateTime EffectiveDateTime { get; set; }

        [UIHint("EditDate")]
        public DateTime EffectiveEndDateTime { get; set; }

        public string Location { get; set; }

        public Collection<string> Brands { get; set; }

        public Collection<string> Tags { get; set; }
    }
}