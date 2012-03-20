using System;
using System.Xml.Serialization;
using Ramone.HyperMedia;
using System.Collections.Generic;


namespace Ramone.MediaTypes.Atom
{
  /// <summary>
  /// Represents an ATOM feed link.
  /// </summary>
  /// <remarks>Is similar to .NET's built in SyndicationItem, but this one is XML serializable as a ATOM link.</remarks>
  public class AtomLink : SelectableBase, ILink
  {
    [XmlIgnore()]
    public Uri HRef { get; set; }


    [XmlAttribute("href")]
    public string HRefText
    {
      get { return HRef != null ? HRef.AbsoluteUri : null; }
      set { HRef = new Uri(value); }
    }


    /// <summary>
    /// Space separated relation types. For XML serialization.
    /// </summary>
    [XmlAttribute("rel")]
    public string RelationType
    {
      get { return GetRelationType(); }
      set { SetRelationType(value); }
    }


    [XmlIgnore()]
    public IEnumerable<string> RelationTypes
    {
      get { return GetRelationTypes(); }
    }

    
    [XmlAttribute("type")]
    public string MediaType { get; set; }

    
    [XmlAttribute("title")]
    public string Title { get; set; }


    public AtomLink()
    {
    }


    public AtomLink(Uri baseUrl, string href, string relationType, MediaType mediaType, string title)
      : this(new Uri(baseUrl, href), relationType, mediaType != null ? mediaType.FullType : null, title)
    {
    }


    public AtomLink(Uri baseUrl, string href, string relationType, string mediaType, string title)
      : this(new Uri(baseUrl, href), relationType, mediaType, title)
    {
    }


    public AtomLink(Uri href, string relationType, MediaType mediaType, string title)
      : this(href, relationType, mediaType != null ? mediaType.FullType : null, title)
    {
    }


    public AtomLink(Uri href, string relationType, string mediaType, string title)
    {
      HRef = href;
      RelationType = relationType;
      MediaType = mediaType;
      Title = title;
    }
  }
}
