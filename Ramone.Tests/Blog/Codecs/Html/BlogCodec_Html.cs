using System.Linq;
using HtmlAgilityPack;
using System.Collections.Generic;
using Ramone.HyperMedia.Html;


namespace Ramone.Tests.Blog.Codecs.Html
{
  public class BlogCodec_Html : BaseCodec_Html<Resources.Blog>
  {
    public override Resources.Blog ReadFromHtml(HtmlDocument html)
    {
      HtmlNode doc = html.DocumentNode;

      List<Resources.Blog.Post> posts = new List<Resources.Blog.Post>();
      List<Anchor> links = new List<Anchor>();

      foreach (HtmlNode postNode in doc.SelectNodes(@"//div[@class=""post""]"))
      {
        HtmlNode title = postNode.SelectNodes(@".//*[@class=""post-title""]").First();
        HtmlNode content = postNode.SelectNodes(@".//*[@class=""post-content""]").First();

        posts.Add(new Resources.Blog.Post
        {
          Title = title.InnerText,
          Text = content.InnerText
        });
      }

      HtmlNode authorLinkNode = doc.SelectNodes(@"//a[@rel=""author""]").First();
      links.Add(new Anchor(authorLinkNode.Attributes["href"].Value, "author", null, authorLinkNode.InnerText));

      Resources.Blog blog = new Resources.Blog()
      {
        Title = doc.SelectNodes(@".//*[@class=""blog-title""]").First().InnerText,
        Posts = posts,
        Links = links
      };

      return blog;
    }
  }
}
