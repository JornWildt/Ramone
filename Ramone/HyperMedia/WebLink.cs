using System;
using System.Collections.Generic;


namespace Ramone.HyperMedia
{
  public class WebLink : IParameterizedLink
  {
    public string RelationshipType
    {
      get { return Parameters["rel"]; }
      set { Parameters["rel"] = value; }
    }

    public string MediaType
    {
      get { return Parameters["type"]; }
      set { Parameters["type"] = value; }
    }

    public string HRef
    {
      get { return Parameters["href"]; }
      set { Parameters["href"] = value; }
    }

    public string Title
    {
      get { return Parameters["title"]; }
      set { Parameters["title"] = value; }
    }

    public Dictionary<string, string> Parameters { get; protected set; }


    public WebLink()
    {
      Parameters = new Dictionary<string, string>();
    }


    public WebLink(Uri href, string relationshipType, string mediaType, string title)
      : this(href.ToString(), relationshipType, mediaType, title)
    {
    }


    public WebLink(string href, string relationshipType, string mediaType, string title)
    {
      Parameters = new Dictionary<string, string>();
      HRef = href;
      RelationshipType = relationshipType;
      MediaType = mediaType;
      Title = title;
    }
  }
}
