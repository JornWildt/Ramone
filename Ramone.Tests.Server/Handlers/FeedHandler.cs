using System;
using System.ServiceModel.Syndication;
using OpenRasta.Web;


namespace Ramone.Tests.Server.Handlers
{
  public class FeedHandler
  {
    public object Get(string name)
    {
      if (name == "Unknown")
        return new OperationResult.NotFound();
      SyndicationFeed f = new SyndicationFeed(name, name + "'s fantastic feed", null);
      return f;
    }


    public object Get(string feedname, string itemname)
    {
      SyndicationItem item = new SyndicationItem(itemname, "Blah blah", new Uri("http://xxx.yyy"));
      return item;   
    }
  }
}
