using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;
using HtmlAgilityPack;
using Ramone.HyperMedia.Atom;
using System.Collections;


namespace Ramone.HyperMedia.Html
{
  public static class HtmlExtensions
  {
    public static IEnumerable<ILink> Anchors(this HtmlDocument html)
    {
      if (html == null)
        return Enumerable.Empty<ILink>();

      return html.DocumentNode
                 .SelectNodes("//a")
                 .Select(a => new AtomLink(GetAttribute(a, "href"), GetAttribute(a, "rel"), null, a.InnerText));
    }


    public static IEnumerable<ILink> Anchors(this HtmlNode node)
    {
      if (node == null)
        return Enumerable.Empty<ILink>();

      return node.SelectNodes(".//a")
                 .Select(a => new AtomLink(GetAttribute(a, "href"), GetAttribute(a, "rel"), null, a.InnerText));
    }


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


    public static ILink Anchor(this HtmlNode node)
    {
      Condition.Requires(node, "node").IsNotNull();

      return new AtomLink(GetAttribute(node, "href"), GetAttribute(node, "rel"), null, node.InnerText);
    }



    #region Not really the right stuff

    public static IEnumerable<ILink> Links(this HtmlDocument html)
    {
      Condition.Requires(html, "html").IsNotNull();

      return html.DocumentNode
                 .SelectNodes("//a")
                 .Select(a => new AtomLink(GetAttribute(a, "href"), GetAttribute(a, "rel"), null, a.InnerText));
    }


    public static IEnumerable<ILink> Links(this HtmlNode node)
    {
      Condition.Requires(node, "node").IsNotNull();

      return node.SelectNodes(".//a")
                 .Select(a => new AtomLink(GetAttribute(a, "href"), GetAttribute(a, "rel"), null, a.InnerText));
    }


    public static ILink Link(this HtmlDocument html, string rel)
    {
      Condition.Requires(html, "html").IsNotNull();
      Condition.Requires(rel, "rel").IsNotNull();

      return html.DocumentNode
                 .SelectNodes("//a")
                 .Where(a => GetAttribute(a, "rel") == rel)
                 .Select(a => new AtomLink(GetAttribute(a, "href"), GetAttribute(a, "rel"), null, a.InnerText))
                 .FirstOrDefault();
    }


    public static ILink Link(this HtmlNode node, string rel)
    {
      Condition.Requires(node, "node").IsNotNull();
      Condition.Requires(rel, "rel").IsNotNullOrEmpty();

      return node.SelectNodes(".//a")
                 .Where(a => GetAttribute(a,"rel") == rel)
                 .Select(a => new AtomLink(GetAttribute(a,"href"), GetAttribute(a,"rel"), null, a.InnerText))
                 .FirstOrDefault();
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

    #endregion
  }
}
