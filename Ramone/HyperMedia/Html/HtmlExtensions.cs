using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;
using HtmlAgilityPack;
using Ramone.HyperMedia.Atom;


namespace Ramone.HyperMedia.Html
{
  public static class HtmlExtensions
  {
    /// <summary>
    /// Get all anchors in the HTML document as a sequence of ILink.
    /// </summary>
    /// <param name="html"></param>
    /// <returns></returns>
    public static IEnumerable<ILink> Anchors(this HtmlDocument html)
    {
      if (html == null)
        return Enumerable.Empty<ILink>();

      return html.DocumentNode
                 .SelectNodes("//a")
                 .Select(a => new AtomLink(GetAttribute(a, "href"), GetAttribute(a, "rel"), null, a.InnerText));
    }


    /// <summary>
    /// Get all anchors in the HTML sub-document as a sequence of ILink.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public static IEnumerable<ILink> Anchors(this HtmlNode node)
    {
      if (node == null)
        return Enumerable.Empty<ILink>();

      return node.SelectNodes(".//a")
                 .Select(a => new AtomLink(GetAttribute(a, "href"), GetAttribute(a, "rel"), null, a.InnerText));
    }


    /// <summary>
    /// Get all anchors, in all HTML sub-documents, as a sequence of ILink.
    /// </summary>
    /// <param name="nodes"></param>
    /// <returns></returns>
    public static IEnumerable<ILink> Anchors(this HtmlNodeCollection nodes)
    {
      if (nodes == null)
        return Enumerable.Empty<ILink>();

      var anchors = 
        from c in nodes
        from a in c.SelectNodes(".//a")
        select new AtomLink(GetAttribute(a, "href"), GetAttribute(a, "rel"), null, a.InnerText);

      return anchors;
    }


    /// <summary>
    /// Convert a single HTML node to ILink, assuming the node represents a anchor.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public static ILink Anchor(this HtmlNode node)
    {
      Condition.Requires(node, "node").IsNotNull();

      return new AtomLink(GetAttribute(node, "href"), GetAttribute(node, "rel"), null, node.InnerText);
    }


    private static string GetAttribute(HtmlNode node, string name)
    {
      if (node == null)
        return null;

      HtmlAttribute a = node.Attributes[name];
      if (a == null)
        return null;

      return a.Value;
    }
  }
}
