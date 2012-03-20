using System;
using Ramone.HyperMedia;


namespace Ramone.MediaTypes.Html
{
  public class Anchor : LinkBase
  {
    public Anchor()
    {
    }


    public Anchor(Uri baseUrl, string href, string relationType, MediaType mediaType, string title)
      : this(baseUrl, href, relationType, mediaType != null ? mediaType.FullType : null, title)
    {
    }


    public Anchor(Uri baseUrl, string href, string relationType, string mediaType, string title)
      : this(baseUrl != null ? new Uri(baseUrl, href) : new Uri(href), relationType, mediaType, title)
    {
    }


    public Anchor(Uri href, string relationType, MediaType mediaType, string title)
      : this(href, relationType, mediaType != null ? mediaType.FullType : null, title)
    {
    }


    public Anchor(Uri href, string relationType, string mediaType, string title)
      : base(href, relationType, mediaType, title)
    {
    }
  }
}
