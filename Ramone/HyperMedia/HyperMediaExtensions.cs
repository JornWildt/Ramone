﻿using System.Linq;
using CuttingEdge.Conditions;
using System.Collections.Generic;
using System;


namespace Ramone.HyperMedia
{
  public static class HyperMediaExtensions
  {
    public static RamoneRequest Request(this IRamoneSession session, ILink link)
    {
      return new RamoneRequest(session, link.HRef);
    }


    /// <summary>
    /// Select a link identified by link relation and optional media-type.
    /// </summary>
    /// <param name="links"></param>
    /// <param name="rel"></param>
    /// <param name="mediaType"></param>
    /// <returns></returns>
    public static ILink Select(this IEnumerable<ILink> links, string rel, string mediaType = null)
    {
      Condition.Requires(links, "links").IsNotNull();
      Condition.Requires(rel, "rel").IsNotNull();

      return links.Where(l => l.RelationshipType == rel && (mediaType == null || l.MediaType == mediaType)).FirstOrDefault();
    }


    /// <summary>
    /// Create request from link.
    /// </summary>
    /// <param name="link"></param>
    /// <param name="session"></param>
    /// <returns></returns>
    public static RamoneRequest Follow(this ILink link, IRamoneSession session)
    {
      Condition.Requires(link, "link").IsNotNull();
      Condition.Requires(session, "session").IsNotNull();

      return new RamoneRequest(session, link.HRef);
    }


    /// <summary>
    /// Create request from URI.
    /// </summary>
    /// <param name="url"></param>
    /// <param name="session"></param>
    /// <returns></returns>
    public static RamoneRequest Follow(this Uri url, IRamoneSession session)
    {
      Condition.Requires(url, "url").IsNotNull();
      Condition.Requires(session, "session").IsNotNull();

      return new RamoneRequest(session, url);
    }


    /// <summary>
    /// Create request from link identified by link relation and optional media-type.
    /// </summary>
    /// <param name="links"></param>
    /// <param name="session"></param>
    /// <param name="rel"></param>
    /// <param name="mediaType"></param>
    /// <returns></returns>
    public static RamoneRequest Follow(this IEnumerable<ILink> links, IRamoneSession session, string rel, string mediaType = null)
    {
      Condition.Requires(links, "links").IsNotNull();
      Condition.Requires(session, "session").IsNotNull();
      Condition.Requires(rel, "rel").IsNotNull();

      ILink link = links.Select(rel, mediaType);
      if (link == null)
        return null;

      return session.Request(link);
    }
  }
}
