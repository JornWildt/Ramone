﻿using System;
using Ramone.HyperMedia;


namespace Ramone.MediaTypes.Html
{
  public class Link : LinkBase
  {
    public Link()
    {
    }


    public Link(Uri baseUrl, string href, string relationType, MediaType mediaType, string title)
      : base(baseUrl, href, relationType, mediaType, title)
    {
    }


    public Link(Uri href, string relationType, MediaType mediaType, string title)
      : base(href, relationType, mediaType, title)
    {
    }
  }
}
