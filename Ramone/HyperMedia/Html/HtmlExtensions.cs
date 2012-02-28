using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;
using HtmlAgilityPack;


namespace Ramone.HyperMedia.Html
{
  public static class HtmlExtensions
  {
    /// <summary>
    /// Get all anchors in the HTML document as a sequence of ILink.
    /// </summary>
    /// <param name="html"></param>
    /// <returns></returns>
    public static IEnumerable<Anchor> Anchors(this HtmlDocument html)
    {
      if (html == null)
        return Enumerable.Empty<Anchor>();

      return html.DocumentNode
                 .SelectNodes("//a")
                 .Select(a => new Anchor(a.GetAttributeValue("href", null), a.GetAttributeValue("rel", null), null, a.InnerText));
    }


    /// <summary>
    /// Get all anchors in the HTML sub-document as a sequence of ILink.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public static IEnumerable<Anchor> Anchors(this HtmlNode node)
    {
      if (node == null)
        return Enumerable.Empty<Anchor>();

      return node.SelectNodes(".//a")
                 .Select(a => new Anchor(a.GetAttributeValue("href", null), a.GetAttributeValue("rel", null), null, a.InnerText));
    }


    /// <summary>
    /// Get all anchors, in all HTML sub-documents, as a sequence of ILink.
    /// </summary>
    /// <param name="nodes"></param>
    /// <returns></returns>
    public static IEnumerable<Anchor> Anchors(this HtmlNodeCollection nodes)
    {
      if (nodes == null)
        return Enumerable.Empty<Anchor>();

      var anchors = 
        from c in nodes
        from a in c.SelectNodes(".//a")
        select new Anchor(a.GetAttributeValue("href", null), a.GetAttributeValue("rel", null), null, a.InnerText);

      return anchors;
    }


    /// <summary>
    /// Convert a single HTML node to ILink, assuming the node represents a anchor.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public static Anchor Anchor(this HtmlNode node)
    {
      Condition.Requires(node, "node").IsNotNull();

      return new Anchor(node.GetAttributeValue("href", null), node.GetAttributeValue("rel", null), null, node.InnerText);
    }


    public static Form Form(this HtmlNode node)
    {
      return new Form(node);
    }


    //internal static string GetAttribute(this HtmlNode node, string name)
    //{
    //  if (node == null)
    //    return null;

    //  HtmlAttribute a = node.Attributes[name];
    //  if (a == null)
    //    return null;

    //  return a.Value;
    //}
  }
}
