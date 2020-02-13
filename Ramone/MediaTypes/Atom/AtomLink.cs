using Newtonsoft.Json;
using Ramone.HyperMedia;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Ramone.MediaTypes.Atom
{
  /// <summary>
  /// Represents an ATOM feed link.
  /// </summary>
  /// <remarks>Is similar to .NET's built in SyndicationItem, but this one is XML serializable as a ATOM link.</remarks>
  public class AtomLink : SelectableBase, ISessionLink, IHaveContext
  {
    [XmlIgnore]
    [JsonIgnore]
    public Uri HRef { get; set; }


    [XmlAttribute("href")]
    public string HRefText
    {
      get { return HRef != null ? HRef.ToString() : null; }
      set { HRef = new Uri(value, UriKind.RelativeOrAbsolute); }
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


    [XmlIgnore]
    [JsonIgnore]
    public IEnumerable<string> RelationTypes
    {
      get { return GetRelationTypes(); }
    }


    [XmlAttribute("type")]
    public string MediaTypeText
    {
      get { return GetMediaTypeText(); }
      set { SetMediaType(value); }
    }


    [XmlIgnore]
    [JsonIgnore]
    public MediaType MediaType
    {
      get { return GetMediaType(); }
      set { SetMediaType(value); }
    }


    [XmlAttribute("title")]
    public string Title { get; set; }


    [XmlIgnore]
    [JsonIgnore]
    public ISession Session { get; set; }


    public AtomLink()
    {
    }


    /// <summary>
    /// Create ATOM link from absolute or relative URI.
    /// </summary>
    /// <param name="href"></param>
    /// <param name="relationType"></param>
    /// <param name="mediaType"></param>
    /// <param name="title"></param>
    public AtomLink(Uri href, string relationType, MediaType mediaType, string title)
    {
      HRef = href;
      RelationType = relationType;
      MediaType = mediaType;
      Title = title;
    }


    /// <summary>
    /// Create ATOM link from base URI and path.
    /// </summary>
    /// <param name="baseUrl">Base URI - can be null</param>
    /// <param name="href"></param>
    /// <param name="relationType"></param>
    /// <param name="mediaType"></param>
    /// <param name="title"></param>
    public AtomLink(Uri baseUrl, string href, string relationType, MediaType mediaType, string title)
      : this(baseUrl != null ? new Uri(baseUrl, href) : new Uri(href), relationType, mediaType, title)
    {
    }


    /// <summary>
    /// Create ATOM link from relative path or absolute URI.
    /// </summary>
    /// <param name="href"></param>
    /// <param name="relationType"></param>
    /// <param name="mediaType"></param>
    /// <param name="title"></param>
    public AtomLink(string href, string relationType, MediaType mediaType, string title)
      : this(new Uri(href, UriKind.RelativeOrAbsolute), relationType, mediaType, title)
    {
    }

    #region IHaveContext Members

    public void RegisterContext(ISession session, Uri baseUrl)
    {
      if (HRef != null)
        HRef = new Uri(baseUrl, HRef);
      Session = session;
    }

    #endregion
  }
}
