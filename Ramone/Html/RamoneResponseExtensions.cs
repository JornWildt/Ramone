using System.Xml;


namespace Ramone.Html
{
  public static class RamoneResponseExtensions
  {
    public static HtmlForm SelectHtmlForm(this RamoneResponse<XmlDocument> response, string formSelector)
    {
      return response.Body.SelectHtmlForm(formSelector, response.BaseUri, response.Session);
    }


    public static HtmlAnchor SelectHtmlAnchor(this RamoneResponse<XmlDocument> response, string anchorSelector)
    {
      return response.Body.SelectHtmlAnchor(anchorSelector, response.BaseUri);
    }
  }
}
