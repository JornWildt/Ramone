using System;
using System.Collections.Generic;


namespace Ramone.HyperMedia
{
  public class WebLink : SelectableBase, IParameterizedLink
  {
    public string HRef
    {
      get { return Parameters["href"]; }
      set { Parameters["href"] = value; }
    }


    public string RelationType
    {
      get { return Parameters["rel"]; }
      set 
      { 
        SetRelationType(value); 
        Parameters["rel"] = value; 
      }
    }


    public IEnumerable<string> RelationTypes
    {
      get { return GetRelationTypes(); }
    }


    public string MediaType
    {
      get { return Parameters["type"]; }
      set { Parameters["type"] = value; }
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


    public WebLink(Uri href, string relationType, string mediaType, string title)
      : this(href.AbsoluteUri, relationType, mediaType, title)
    {
    }


    public WebLink(Uri href, string relationType, MediaType mediaType, string title)
      : this(href.AbsoluteUri, relationType, mediaType != null ? mediaType.FullType : null, title)
    {
    }


    public WebLink(string href, string relationType, MediaType mediaType, string title)
      : this(href, relationType, mediaType != null ? mediaType.FullType : null, title)
    {
    }


    public WebLink(string href, string relationshipType, string mediaType, string title)
    {
      Parameters = new Dictionary<string, string>();
      HRef = href;
      RelationType = relationshipType;
      MediaType = mediaType;
      Title = title;
    }
  }
}
