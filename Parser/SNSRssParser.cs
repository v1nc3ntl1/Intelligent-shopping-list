using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
  using System.Collections.ObjectModel;
  using System.Text.RegularExpressions;
  using DomainObject;
  using Framework;
  using HigLabo.Rss;

  public class SNSRssParser<T, TN> : IParser<Promotion, RssFeed>
  {
    private IParser<Promotion, RssFeed> _parser;

    public SNSRssParser() { }

    public SNSRssParser(IParser<Promotion, RssFeed> parser)
    {
      _parser = parser;
    }

    public IEnumerable<Promotion> Parse(RssFeed input)
    {
      Collection<Promotion> coll = new Collection<Promotion>();

      Promotion currentPromotion = null;
      foreach (var item in input.Items)
      {
        if (item.GetType() == typeof(RssItem_2_0))
        {
          currentPromotion = new Promotion()
          {
            PromotionName = item.Title,
            Description = item.Description,
            PromotionItems = new Collection<Item>(),
            Link = item.Link,
            Html = item.MiscData.ContainsKey("encoded") ? item.MiscData["encoded"] : "",
            EffectiveDateTime = item.PubDate.HasValue ? item.PubDate.Value.Date : DateTime.MinValue
          };

          var temp = item as RssItem_2_0;

          if (temp != null)
          {
            if (!temp.Categories.IsNullOrEmpty())
            {
              foreach (var category in temp.Categories)
              {
                currentPromotion.PromotionItems.Add(new Item()
                {
                  ItemName = category.Title,
                  Tag = category.Title
                });
              }
            }
          }

          if (!string.IsNullOrEmpty(currentPromotion.Description))
          {
            //Match the location
            //Regex regexLocation = new Regex("([L|l]ocation)", RegexOptions.ECMAScript);
            //regexLocation.Matches()
          }

          coll.Add(currentPromotion);
        }

        //sb.AppendFormat("{{{0}}},{{{1}}},{{{2}}}", item.Description, item.Link, item.Title);
      }

      if (_parser != null)
      {
        var additionalResult = _parser.Parse(input);
        if (additionalResult != null)
        {
          foreach (var a in additionalResult)
          {
            coll.Add(a);  
          }  
        }
      }
      return coll;
    }
  }
}
