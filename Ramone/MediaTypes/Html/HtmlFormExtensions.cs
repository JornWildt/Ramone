using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;
using HtmlAgilityPack;
using System;


namespace Ramone.MediaTypes.Html
{
  public static class HtmlFormExtensions
  {
    public static Form Form(this HtmlNode node, ISession session, Uri baseUri, string responseCharset)
    {
      return new Form(node, session, baseUri, responseCharset);
    }


    public static Form Form(this HtmlNode node, Response response)
    {
      return new Form(node, response.Session, response.BaseUri, response.WebResponse.CharacterSet);
    }
  }
}
