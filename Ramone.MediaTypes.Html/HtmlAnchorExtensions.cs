﻿using System.Collections.Generic;
using System.Linq;
using Ramone.Utility.Validation;
using HtmlAgilityPack;
using System;


namespace Ramone.MediaTypes.Html
{
  public static class HtmlAnchorExtensions
  {
    /// <summary>
    /// Get all anchors in the HTML document as a sequence of ILink.
    /// </summary>
    /// <param name="html"></param>
    /// <param name="response">The response from which the html originates. 
    /// The response information is used to convert relative links to absolute links.</param>
    /// <returns></returns>
    public static IEnumerable<Anchor> Anchors(this HtmlDocument html, Response response)
    {
      return Anchors(html, response != null ? response.BaseUri : null);
    }


    public static IEnumerable<Anchor> Anchors(this HtmlDocument html, Uri baseUrl)
    {
      if (html == null)
        return Enumerable.Empty<Anchor>();

      return html.DocumentNode
                 .SelectNodes("//a")
                 .Select(a => new Anchor(baseUrl, a.GetAttributeValue("href", null), a.GetAttributeValue("rel", null), (string)null, a.InnerText));
    }


    /// <summary>
    /// Get all anchors in the HTML sub-document as a sequence of ILink.
    /// </summary>
    /// <param name="node"></param>
    /// <param name="response">The response from which the html originates. 
    /// The response information is used to convert relative links to absolute links.</param>
    /// <returns></returns>
    public static IEnumerable<Anchor> Anchors(this HtmlNode node, Response response)
    {
      return Anchors(node, response != null ? response.BaseUri : null);
    }


    public static IEnumerable<Anchor> Anchors(this HtmlNode node, Uri baseUrl)
    {
      if (node == null)
        return Enumerable.Empty<Anchor>();

      return node.SelectNodes(".//a")
                 .Select(a => new Anchor(baseUrl, a.GetAttributeValue("href", null), a.GetAttributeValue("rel", null), (string)null, a.InnerText));
    }


    /// <summary>
    /// Get all anchors, in all HTML sub-documents, as a sequence of ILink.
    /// </summary>
    /// <param name="nodes"></param>
    /// <param name="response">The response from which the html originates. 
    /// The response information is used to convert relative links to absolute links.</param>
    /// <returns></returns>
    public static IEnumerable<Anchor> Anchors(this HtmlNodeCollection nodes, Response response)
    {
      return Anchors(nodes, response != null ? response.BaseUri : null);
    }


    public static IEnumerable<Anchor> Anchors(this HtmlNodeCollection nodes, Uri baseUrl)
    {
      if (nodes == null)
        return Enumerable.Empty<Anchor>();

      var anchors =
        from c in nodes
        from a in c.SelectNodes(".//a")
        select new Anchor(baseUrl, a.GetAttributeValue("href", null), a.GetAttributeValue("rel", null), (string)null, a.InnerText);

      return anchors;
    }


    /// <summary>
    /// Convert a single HTML node to ILink, assuming the node represents a anchor.
    /// </summary>
    /// <param name="node"></param>
    /// <param name="response">The response from which the html originates. 
    /// The response information is used to convert relative links to absolute links.</param>
    /// <returns></returns>
    public static Anchor Anchor(this HtmlNode node, Response response)
    {
      return Anchor(node, response != null ? response.BaseUri : null);
    }


    public static Anchor Anchor(this HtmlNode node, Uri baseUrl)
    {
      Condition.Requires(node, "node").IsNotNull();

      return new Anchor(baseUrl, node.GetAttributeValue("href", null), node.GetAttributeValue("rel", null), (string)null, node.InnerText);
    }
  }
}
