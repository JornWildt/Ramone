using System;
using System.Xml;


namespace Ramone
{
  public class HtmlAnchor
  {
    public Uri HRef { get; set; }

    public string Rel { get; set; }


    public HtmlAnchor(Uri baseUri, XmlNode anchorXml)
    {
      HRef = baseUri;

      if (anchorXml.Attributes["href"] != null)
        HRef = new Uri(baseUri, anchorXml.Attributes["href"].Value);
    }
  }
}
