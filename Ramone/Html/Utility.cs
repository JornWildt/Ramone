using System;
using System.Xml;
using System.Xml.XPath;


namespace Ramone.Html
{
  public static class Utility
  {
    public static string ConvertSelectorToXPath(string selector)
    {
      if (selector.StartsWith("#"))
        return string.Format("//*[@id=\"{0}\"]", selector.Substring(1));
      else
        return selector;
    }


    public static XmlNode SelectSingleNode(XmlNode xml, string selector)
    {
      string path = ConvertSelectorToXPath(selector);
      try
      {
        XmlNode node = xml.SelectSingleNode(path);
        return node;
      }
      catch (XPathException ex)
      {
        throw new XPathException(string.Format("{0}\nSelector = '{1}', XPath = '{2}', XML = {3}", ex.Message, selector, path, xml));
      }
    }
  }
}
