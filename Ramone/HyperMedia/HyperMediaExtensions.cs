using System.Linq;
using CuttingEdge.Conditions;
using System.Collections.Generic;
using System;


namespace Ramone.HyperMedia
{
  public static class HyperMediaExtensions
  {
    /// <summary>
    /// Select a link identified by link relation and optional media-type.
    /// </summary>
    /// <param name="links"></param>
    /// <param name="rel"></param>
    /// <param name="mediaType"></param>
    /// <returns>Selected link or null if none was found.</returns>
    public static T Select<T>(this IEnumerable<T> links, string rel, MediaType mediaType = null)
      where T : ISelectable
    {
      Condition.Requires(links, "links").IsNotNull();
      Condition.Requires(rel, "rel").IsNotNull();

      return links.Where(l => l.RelationTypes.Any(r => string.Equals(r, rel, StringComparison.InvariantCultureIgnoreCase))
                              && (mediaType == null || l.MediaType == mediaType)).FirstOrDefault();
    }


    /// <summary>
    /// Create request from link and specific session.
    /// </summary>
    /// <param name="link"></param>
    /// <param name="session"></param>
    /// <returns></returns>
    public static Request Follow(this ILink link, ISession session)
    {
      Condition.Requires(link, "link").IsNotNull();
      Condition.Requires(session, "session").IsNotNull();

      return new Request(session, link.HRef);
    }


    /// <summary>
    /// Create request from link, apply base URL from specific response and use session from same response.
    /// </summary>
    /// <param name="response"></param>
    /// <param name="link"></param>
    /// <returns></returns>
    public static Request Follow(this Response response, ILink link)
    {
      Condition.Requires(response, "response").IsNotNull();
      Condition.Requires(link, "link").IsNotNull();

      return new Request(response.Session, new Uri(response.BaseUri, link.HRef));
    }


    /// <summary>
    /// Create request from absolute link containing its own session reference.
    /// </summary>
    /// <param name="link"></param>
    /// <returns></returns>
    public static Request Follow(this ISessionLink link)
    {
      Condition.Requires(link, "link").IsNotNull();

      return new Request(link.Session, link.HRef);
    }


    /// <summary>
    /// Create request from URI and specific session.
    /// </summary>
    /// <param name="url"></param>
    /// <param name="session"></param>
    /// <returns></returns>
    public static Request Follow(this Uri url, ISession session)
    {
      Condition.Requires(url, "url").IsNotNull();
      Condition.Requires(session, "session").IsNotNull();

      return new Request(session, url);
    }


    /// <summary>
    /// Create request from link identified by link relation and optional media-type.
    /// </summary>
    /// <param name="links"></param>
    /// <param name="session"></param>
    /// <param name="rel"></param>
    /// <param name="mediaType"></param>
    /// <returns></returns>
    //public static Request Follow(this IEnumerable<ILink> links, ISession session, string rel, MediaType mediaType = null)
    //{
    //  Condition.Requires(links, "links").IsNotNull();
    //  Condition.Requires(session, "session").IsNotNull();
    //  Condition.Requires(rel, "rel").IsNotNull();

    //  ILink link = links.Select(rel, mediaType);
    //  if (link == null)
    //    return null;

    //  return new Request(session, link.HRef);
    //}

  
  }
}
