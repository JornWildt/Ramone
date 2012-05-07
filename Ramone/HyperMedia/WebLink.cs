using System;
using System.Collections.Generic;


namespace Ramone.HyperMedia
{
  public class WebLink : SelectableBase, IParameters, ILink
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


    public MediaType MediaType
    {
      get { return Parameters["type"]; }
      set 
      { 
        SetMediaType(value);
        Parameters["type"] = (value != null ? (string)value : null);
      }
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


    /// <summary>
    /// Create web link from absolute URI.
    /// </summary>
    /// <param name="href"></param>
    /// <param name="relationType"></param>
    /// <param name="mediaType"></param>
    /// <param name="title"></param>
    public WebLink(Uri href, string relationType, MediaType mediaType, string title)
      : this(href.AbsoluteUri, relationType, mediaType, title)
    {
    }


    /// <summary>
    /// Create web link from base URI and path.
    /// </summary>
    /// <param name="baseUrl">Base URI - can be null</param>
    /// <param name="href"></param>
    /// <param name="relationType"></param>
    /// <param name="mediaType"></param>
    /// <param name="title"></param>
    public WebLink(Uri baseUrl, string href, string relationType, MediaType mediaType, string title)
      : this(baseUrl != null ? new Uri(baseUrl, href) : new Uri(href), relationType, mediaType, title)
    {
    }


    /// <summary>
    /// Create web link from relative path or absolute URI.
    /// </summary>
    /// <param name="href"></param>
    /// <param name="relationshipType"></param>
    /// <param name="mediaType"></param>
    /// <param name="title"></param>
    public WebLink(string href, string relationshipType, MediaType mediaType, string title)
    {
      Parameters = new Dictionary<string, string>();
      HRef = href;
      RelationType = relationshipType;
      MediaType = mediaType;
      Title = title;
    }
  }
}
