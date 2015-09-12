using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest
{
  using BusinessLogic;
  using DomainObject;
  using Framework;
  using HigLabo.Rss;
  using Parser;

  class Program
  {
    static void Main(string[] args)
    {
      //GetRSSPromotion();
      DemoPullPromotion();
      Console.Read();
    }

    private static void GetRSSPromotion()
    {
      var rssFeed = External.HigLabFascade.GetRss("http://www.shoppingnsales.com/feed/");
      if (rssFeed != null)
      {
        var parser = new SNSRssParser<Promotion, RssFeed>();
        var result = parser.Parse(rssFeed);
        if (result != null)
        {
          foreach (var item in result)
          {
            Console.WriteLine("PromotionName  :" + item.PromotionName);
            Console.WriteLine("Description  :" + item.Description);
            if (item.PromotionItems != null)
            {
              foreach (Item promotionItem in item.PromotionItems)
              {
                Console.WriteLine("Category  :" + promotionItem.Tag);
              }
            }
            ;
            Console.WriteLine("-------------------------------------");
          }
        }
      }
    }

    async private static void DemoPullPromotion()
    {
      var rssFeed = External.HigLabFascade.GetRss("http://www.shoppingnsales.com/feed/");

      if (rssFeed != null)
      {
        var parser = new SNSRssParser<Promotion, RssFeed>();
        var result = parser.Parse(rssFeed);
        if (result != null)
        {
          var creator = SpringResolver.GetObject<IPromotionCreator>("PromotionCreatorImpl");

          Promotion promotion;
          foreach (var item in result)
          {
            promotion = new Promotion()
            {
              PromotionName = item.PromotionName,
              EffectiveDateTime = item.EffectiveDateTime,
              Description = item.Description,
              PromotionItems = item.PromotionItems
            };

            await creator.SavePromotion(promotion);
          }
        }
      }
    }
  }
}
