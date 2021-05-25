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
      : base(baseUrl, href, relationType, mediaType, title)
    {
    }


    public Anchor(Uri href, string relationType, MediaType mediaType, string title)
      : base(href, relationType, mediaType, title)
    {
    }
  }
}
