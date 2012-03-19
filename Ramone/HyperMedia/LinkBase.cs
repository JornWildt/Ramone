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
    public string HRef { get; private set; }

    
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


    public LinkBase(Uri href, string relationshipType, string mediaType, string title)
      : this(href.ToString(), relationshipType, mediaType, title)
    {
    }


    public LinkBase(string href, string relationshipType, string mediaType, string title)
    {
      HRef = href;
      RelationType = relationshipType;
      MediaType = mediaType;
      Title = title;
    }
  }
}
