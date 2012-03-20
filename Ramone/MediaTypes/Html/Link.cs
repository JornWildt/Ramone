using System;
using Ramone.HyperMedia;


namespace Ramone.MediaTypes.Html
{
  public class Link : LinkBase
  {
    public Link()
    {
    }


    public Link(Uri baseUrl, string href, string relationType, MediaType mediaType, string title)
      : this(baseUrl, href, relationType, mediaType != null ? mediaType.FullType : null, title)
    {
    }


    public Link(Uri baseUrl, string href, string relationType, string mediaType, string title)
      : this(baseUrl != null ? new Uri(baseUrl, href) : new Uri(href), relationType, mediaType, title)
    {
    }


    public Link(Uri href, string relationType, MediaType mediaType, string title)
      : this(href, relationType, mediaType != null ? mediaType.FullType : null, title)
    {
    }


    public Link(Uri href, string relationType, string mediaType, string title)
      : base(href, relationType, mediaType, title)
    {
    }
  }
}
