using System;
using Ramone.HyperMedia;


namespace Ramone.MediaTypes.Html
{
  public class Link : LinkBase
  {
    public Link()
    {
    }


    public Link(Uri href, string relationshipType, string mediaType, string title)
      : this(href.ToString(), relationshipType, mediaType, title)
    {
    }


    public Link(string href, string relationshipType, string mediaType, string title)
      : base(href, relationshipType, mediaType, title)
    {
    }
  }
}
