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

      T result =  links.Where(l => l.RelationTypes.Any(r => string.Equals(r, rel, StringComparison.InvariantCultureIgnoreCase))
                                                       && (mediaType == null || l.MediaType == mediaType)).FirstOrDefault();

      if (result == null)
        throw new SelectFailedException(string.Format("No {0} found matching rel='{1}' and mediaType='{2}'.", typeof(T), rel, mediaType));

      return result;
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


    public static Request Follow<T>(this IEnumerable<T> links, ISession session, string rel, MediaType mediaType = null)
      where T : ILink
    {
      return links.Select(rel, mediaType).Follow(session);
    }


    public static Request Follow<T>(this IEnumerable<T> links, string rel, MediaType mediaType = null)
      where T : ISessionLink
    {
      return links.Select(rel, mediaType).Follow();
    }


    /// <summary>
    /// Create request from link with implicit session (created on demand)
    /// </summary>
    /// <param name="link"></param>
    /// <returns></returns>
    public static Request Follow(this ILink link)
    {
      Condition.Requires(link, "link").IsNotNull();

      return new Request(link.HRef);
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
  }
}
