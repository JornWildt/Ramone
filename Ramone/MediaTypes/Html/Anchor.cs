﻿using System;
using Ramone.HyperMedia;


namespace Ramone.MediaTypes.Html
{
  public class Anchor : LinkBase
  {
    public Anchor()
    {
    }


    public Anchor(Uri href, string relationshipType, string mediaType, string title)
      : this(href.ToString(), relationshipType, mediaType, title)
    {
    }


    public Anchor(string href, string relationshipType, string mediaType, string title)
      : base(href, relationshipType, mediaType, title)
    {
    }
  }
}
