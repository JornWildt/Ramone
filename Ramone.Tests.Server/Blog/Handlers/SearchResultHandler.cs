using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using Ramone.Tests.Server.Blog.Resources;


namespace Ramone.Tests.Server.Blog.Handlers
{
  public class SearchResultHandler
  {
    public object Get(string q)
    {
      SyndicationItem item = new SyndicationItem("Result 1", "Blah", new Uri("http://dr.dk"));
      List<SyndicationItem> items = new List<SyndicationItem>();
      items.Add(item);

      SyndicationFeed result = new SyndicationFeed(items);
      
      return new SearchResult
      {
        Result = result
      };
    }
  }
}