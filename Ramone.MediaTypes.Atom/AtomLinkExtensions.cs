using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using Ramone.HyperMedia;
using Ramone.Utility.Validation;


namespace Ramone.MediaTypes.Atom
{
  public static class AtomLinkExtensions
  {
    /// <summary>
    /// Get all links in the syndication feed as a sequence of ILink. Do not include links from feed items.
    /// </summary>
    public static IEnumerable<AtomLink> Links(this SyndicationFeed feed)
    {
      if (feed == null)
        return Enumerable.Empty<AtomLink>();

      return feed.Links.Links();
    }


    /// <summary>
    /// Get all links in the syndication item as a sequence of ILink.
    /// </summary>
    public static IEnumerable<AtomLink> Links(this SyndicationItem item)
    {
      if (item == null)
        return Enumerable.Empty<AtomLink>();

      return item.Links.Links();
    }


    public static AtomLink Link(this SyndicationLink link)
    {
      return new AtomLink(link.Uri, link.RelationshipType, link.MediaType, link.Title);
    }


    public static IEnumerable<AtomLink> Links(this IEnumerable<SyndicationLink> links)
    {
      return links.Select(l => new AtomLink(l.Uri, l.RelationshipType, l.MediaType, l.Title));
    }


    public static AtomLink Select(this IEnumerable<SyndicationLink> links, string rel, MediaType mediaType = null)
    {
      return links.Select(l => l.Link()).Select(rel, mediaType);
    }


    //public static Request Follow(this IEnumerable<SyndicationLink> links, ISession session, string rel, MediaType mediaType = null)
    //{
    //  return links.Select(l => l.Link()).Follow(session, rel, mediaType);
    //}


    public static Request Follow(this SyndicationLink link, ISession session)
    {
      Condition.Requires(link, "link").IsNotNull();
      Condition.Requires(session, "session").IsNotNull();

      return new Request(session, link.Uri);
    }
  }
}
