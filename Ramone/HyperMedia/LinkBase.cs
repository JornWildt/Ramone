using System;
using System.Collections.Generic;


namespace Ramone.HyperMedia
{
  /// <summary>
  /// A generic implementation of ILink.
  /// </summary>
  /// <remarks>Not suitable for direct XML serialization since it has no XML markup attributes for any
  /// known formats.</remarks>
  public abstract class LinkBase : SelectableBase, ILink
  {
    public Uri HRef { get; private set; }

    
    public string RelationType
    {
      get { return GetRelationType(); }
      set { SetRelationType(value); }
    }

    
    public IEnumerable<string> RelationTypes
    {
      get { return GetRelationTypes(); }
    }

    
    public string MediaType { get; private set; }

    
    public string Title { get; private set; }


    public LinkBase()
    {
    }


    public LinkBase(Uri baseUrl, string href, string relationType, string mediaType, string title)
      : this(new Uri(baseUrl, href), relationType, mediaType, title)
    {
    }


    public LinkBase(Uri baseUrl, string href, string relationType, MediaType mediaType, string title)
      : this(new Uri(baseUrl, href), relationType, mediaType != null ? mediaType.FullType : null, title)
    {
    }


    public LinkBase(Uri href, string relationType, MediaType mediaType, string title)
      : this(href, relationType, mediaType != null ? mediaType.FullType : null, title)
    {
    }


    public LinkBase(Uri href, string relationType, string mediaType, string title)
    {
      HRef = href;
      RelationType = relationType;
      MediaType = mediaType;
      Title = title;
    }
  }
}
