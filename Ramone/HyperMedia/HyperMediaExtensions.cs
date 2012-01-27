using System.Linq;
using CuttingEdge.Conditions;
using System.Collections.Generic;


namespace Ramone.HyperMedia
{
  public static class HyperMediaExtensions
  {
    public static RamoneRequest Request(this IRamoneSession session, ILink link)
    {
      return new RamoneRequest(session, link.HRef);
    }


    public static ILink Link(this IEnumerable<ILink> links, string rel)
    {
      Condition.Requires(links, "links").IsNotNull();
      Condition.Requires(rel, "rel").IsNotNull();

      return links.Where(l => l.RelationshipType == rel).FirstOrDefault();
    }


    public static RamoneRequest Follow(this ILink link, IRamoneSession session)
    {
      Condition.Requires(link, "link").IsNotNull();
      Condition.Requires(session, "session").IsNotNull();

      return new RamoneRequest(session, link.HRef);
    }


    public static RamoneRequest Follow(this IEnumerable<ILink> links, IRamoneSession session, string rel)
    {
      Condition.Requires(links, "links").IsNotNull();
      Condition.Requires(session, "session").IsNotNull();
      Condition.Requires(rel, "rel").IsNotNull();

      ILink link = links.Link(rel);
      if (link == null)
        return null;

      return session.Request(link);
    }


    //public static RamoneRequest Follow(this IEnumerable<ILink> links, IRamoneSession session, string rel)
    //{
    //  Condition.Requires(links, "links").IsNotNull();
    //  Condition.Requires(session, "session").IsNotNull();
    //  Condition.Requires(rel, "rel").IsNotNull();

    //  ILink link = links.Link(rel);
    //  if (link == null)
    //    return null;

    //  return session.Request(link);
    //}
  }
}
