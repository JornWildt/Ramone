using System.Linq;
using HtmlAgilityPack;
using System.Collections.Generic;
using Ramone.HyperMedia.Html;
using Ramone.HyperMedia;


namespace Ramone.Tests.Blog.Codecs.Html
{
  public class BlogCodec_Html : BaseCodec_Html<Resources.Blog>
  {
    public override Resources.Blog ReadFromHtml(HtmlDocument html)
    {
      HtmlNode doc = html.DocumentNode;

      List<Resources.Blog.Post> posts = new List<Resources.Blog.Post>();

      foreach (HtmlNode postNode in doc.SelectNodes(@"//div[@class=""post""]"))
      {
        HtmlNode title = postNode.SelectNodes(@".//*[@class=""post-title""]").First();
        HtmlNode content = postNode.SelectNodes(@".//*[@class=""post-content""]").First();
        List<Anchor> links = new List<Anchor>(postNode.Anchors());

        posts.Add(new Resources.Blog.Post
        {
          Title = title.InnerText,
          Text = content.InnerText,
          Links = links
        });
      }

      List<Anchor> blogLinks = new List<Anchor>(doc.Anchors());

      Resources.Blog blog = new Resources.Blog()
      {
        Title = doc.SelectNodes(@".//*[@class=""blog-title""]").First().InnerText,
        Posts = posts,
        Links = blogLinks
      };

      return blog;
    }
  }
}
