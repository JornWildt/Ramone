using System;
using System.Xml.Serialization;
using Ramone.HyperMedia;


namespace Ramone.MediaTypes.Atom
{
  /// <summary>
  /// Represents an ATOM feed link.
  /// </summary>
  /// <remarks>Is similar to .NET's built in SyndicationItem, but this one is serializable as a ATOM link.</remarks>
  public class AtomLink : ILink
  {
    [XmlAttribute("href")]
    public string HRef { get; set; }

    [XmlAttribute("rel")]
    public string RelationshipType { get; set; }

    [XmlAttribute("type")]
    public string MediaType { get; set; }

    [XmlAttribute("title")]
    public string Title { get; set; }


    public AtomLink()
    {
    }


    public AtomLink(Uri href, string relationshipType, string mediaType, string title)
      : this(href.AbsoluteUri, relationshipType, mediaType, title)
    {
    }


    public AtomLink(Uri href, string relationshipType, MediaType mediaType, string title)
      : this(href.AbsoluteUri, relationshipType, mediaType != null ? mediaType.FullType : null, title)
    {
    }


    public AtomLink(string href, string relationshipType, MediaType mediaType, string title)
      : this(href, relationshipType, mediaType != null ? mediaType.FullType : null, title)
    {
    }


    public AtomLink(string href, string relationshipType, string mediaType, string title)
    {
      HRef = href;
      RelationshipType = relationshipType;
      MediaType = mediaType;
      Title = title;
    }
  }
}
