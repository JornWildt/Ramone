using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;


namespace Ramone.MediaTypes.Atom
{
  public static class AtomLinkExtensions
  {
    /// <summary>
    /// Get all links in the syndication feed as a sequence of ILink. Do not include links from feed items.
    /// </summary>
    public static IEnumerable<AtomLink> Links(this SyndicationFeed feed, Response response)
    {
      return Links(feed, response != null ? response.BaseUri : null);
    }


    public static IEnumerable<AtomLink> Links(this SyndicationFeed feed, Uri baseUrl)
    {
      if (feed == null)
        return Enumerable.Empty<AtomLink>();

      return feed.Links.Select(l => new AtomLink(l.Uri, l.RelationshipType, l.MediaType, l.Title));
    }


    /// <summary>
    /// Get all links in the syndication item as a sequence of ILink.
    /// </summary>
    public static IEnumerable<AtomLink> Links(this SyndicationItem item, Response response)
    {
      return Links(item, response != null ? response.BaseUri : null);
    }


    public static IEnumerable<AtomLink> Links(this SyndicationItem item, Uri baseUrl)
    {
      if (item == null)
        return Enumerable.Empty<AtomLink>();

      return item.Links.Select(l => new AtomLink(l.Uri, l.RelationshipType, l.MediaType, l.Title));
    }
  }
}
