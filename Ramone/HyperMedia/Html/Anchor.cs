using System;


namespace Ramone.HyperMedia.Html
{
  public class Anchor : ILink
  {
    public string HRef { get; set; }

    public string RelationshipType { get; set; }

    public string MediaType { get; set; }

    public string Title { get; set; }


    public Anchor()
    {
    }


    public Anchor(Uri href, string relationshipType, string mediaType, string title)
      : this(href.ToString(), relationshipType, mediaType, title)
    {
    }


    public Anchor(string href, string relationshipType, string mediaType, string title)
    {
      HRef = href;
      RelationshipType = relationshipType;
      MediaType = mediaType;
      Title = title;
    }
  }
}
