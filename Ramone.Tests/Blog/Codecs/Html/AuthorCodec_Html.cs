using System.Linq;
using HtmlAgilityPack;


namespace Ramone.Tests.Blog.Codecs.Html
{
  public class AuthorCodec_Html : BaseCodec_Html<Resources.Author>
  {
    public override Resources.Author ReadFromHtml(HtmlDocument html, ReaderContext context)
    {
      HtmlNode doc = html.DocumentNode;

      HtmlNode emailNode = doc.SelectNodes(@"//a[@rel=""email""]").First();
      HtmlNode nameNode = doc.SelectNodes(@"//*[@class=""name""]").First();

      return new Resources.Author
      {
        Name = nameNode.InnerText,
        EMail = emailNode.Attributes["href"].Value
      };
    }
  }
}
