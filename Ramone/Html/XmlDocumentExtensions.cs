using System;
using System.Xml;
using System.Xml.XPath;
using System.Net;


namespace Ramone.Html
{
  public static class XmlDocumentExtensions
  {
    public static HtmlForm SelectHtmlForm(this XmlNode xml, string formSelector, Uri baseUrl, ISession session)
    {
      XmlNode formNode = Utility.SelectSingleNode(xml, formSelector);
      if (formNode == null)
        throw new InvalidOperationException(string.Format("Could not locate <form> element from '{0}'.", formSelector));
      if (!(formNode is XmlElement))
        throw new InvalidOperationException(string.Format("Could not convert XmlNode from '{0}' to XmlElement (was '{1}').", formSelector, formNode.GetType()));
      HtmlForm form = new HtmlForm(baseUrl, formNode, session);
      return form;
    }


    public static HtmlAnchor SelectHtmlAnchor(this XmlNode xml, string anchorSelector, Uri baseUrl)
    {
      XmlNode anchorNode = Utility.SelectSingleNode(xml, anchorSelector);
      if (anchorNode == null)
        throw new InvalidOperationException(string.Format("Could not locate <anchor> element from '{0}'.", anchorSelector));
      if (!(anchorNode is XmlElement))
        throw new InvalidOperationException(string.Format("Could not convert XmlNode from '{0}' to XmlElement (was '{1}').", anchorSelector, anchorNode.GetType()));
      HtmlAnchor anchor = new HtmlAnchor(baseUrl, anchorNode);
      return anchor;
    }
  }
}
