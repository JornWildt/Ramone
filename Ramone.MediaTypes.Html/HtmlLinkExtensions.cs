using System.Collections.Generic;
using System.Linq;
using Ramone.Utility.Validation;
using HtmlAgilityPack;
using System;


namespace Ramone.MediaTypes.Html
{
  public static class HtmlLinkExtensions
  {
    /// <summary>
    /// Get all header links in the HTML document as a sequence of ILink.
    /// </summary>
    /// <param name="html"></param>
    /// <param name="response">The response from which the html originates. 
    /// The response information is used to convert relative links to absolute links.</param>
    /// <returns></returns>
    public static IEnumerable<Link> Links(this HtmlDocument html, Response response)
    {
      return Links(html, response != null ? response.BaseUri : null);
    }


    public static IEnumerable<Link> Links(this HtmlDocument html, Uri baseUrl)
    {
      if (html == null)
        return Enumerable.Empty<Link>();

      return html.DocumentNode
                 .SelectNodes("//head/link")
                 .Select(l => new Link(baseUrl, l.GetAttributeValue("href", null), l.GetAttributeValue("rel", null), l.GetAttributeValue("type", null), l.GetAttributeValue("title", null)));
    }


    /// <summary>
    /// Get all links in the HTML sub-document as a sequence of ILink.
    /// </summary>
    /// <param name="node"></param>
    /// <param name="response">The response from which the html originates. 
    /// The response information is used to convert relative links to absolute links.</param>
    /// <returns></returns>
    public static IEnumerable<Link> Links(this HtmlNode node, Response response)
    {
      return Links(node, response != null ? response.BaseUri : null);
    }


    public static IEnumerable<Link> Links(this HtmlNode node, Uri baseUrl)
    {
      if (node == null)
        return Enumerable.Empty<Link>();

      return node.SelectNodes(".//link")
                 .Select(l => new Link(baseUrl, l.GetAttributeValue("href", null), l.GetAttributeValue("rel", null), l.GetAttributeValue("type", null), l.GetAttributeValue("title", null)));
    }


    /// <summary>
    /// Get all links, in all HTML sub-documents, as a sequence of ILink.
    /// </summary>
    /// <param name="nodes">HtmlNode collection to search for link elements.</param>
    /// <param name="response">The response from which the html originates. 
    /// The response information is used to convert relative links to absolute links.</param>
    /// <returns></returns>
    public static IEnumerable<Link> Links(this HtmlNodeCollection nodes, Response response)
    {
      return Links(nodes, response != null ? response.BaseUri : null);
    }


    public static IEnumerable<Link> Links(this HtmlNodeCollection nodes, Uri baseUrl)
    {
      if (nodes == null)
        return Enumerable.Empty<Link>();

      var links = 
        from n in nodes
        from l in n.SelectNodes(".//link")
        select new Link(baseUrl, l.GetAttributeValue("href", null), l.GetAttributeValue("rel", null), l.GetAttributeValue("type", null), l.GetAttributeValue("title", null));

      return links;
    }


    /// <summary>
    /// Convert a single HTML node to ILink, assuming the node represents a link.
    /// </summary>
    /// <param name="node"></param>
    /// <param name="response">The response from which the html originates. 
    /// The response information is used to convert relative links to absolute links.</param>
    /// <returns></returns>
    public static Link Link(this HtmlNode node, Response response)
    {
      return Link(node, response != null ? response.BaseUri : null);
    }


    public static Link Link(this HtmlNode node, Uri baseUrl)
    {
      Condition.Requires(node, "node").IsNotNull();

      return new Link(baseUrl, node.GetAttributeValue("href", null), node.GetAttributeValue("rel", null), node.GetAttributeValue("type", null), node.GetAttributeValue("title", null));
    }
  }
}
