
namespace External
{
  using System;
  using System.Collections.ObjectModel;
  using DomainObject;
  using Framework;
  using HigLabo.Rss;

  public class HigLabFascade
  {
    public static RssFeed GetRss(string url)
    {
      if (string.IsNullOrEmpty(url))    
        throw new ArgumentNullException("url");

      var cl = new HigLabo.Net.RssClient();

      return cl.GetRssFeed(new Uri(url));
    }

  }
}
