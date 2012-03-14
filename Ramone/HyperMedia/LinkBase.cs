using System;


namespace Ramone.HyperMedia
{
  /// <summary>
  /// A generic implementation of ILink.
  /// </summary>
  /// <remarks>Not suitable for direct XML serialization since it has no XML markup attributes for any
  /// known formats. Use <see cref="AtomLink"/> for XML serializable ATOM links.</remarks>
  public abstract class LinkBase : ILink
  {
    public string HRef { get; private set; }

    public string RelationshipType { get; private set; }

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
      RelationshipType = relationshipType;
      MediaType = mediaType;
      Title = title;
    }
  }
}
