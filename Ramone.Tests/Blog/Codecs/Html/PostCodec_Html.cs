using System.Linq;
using HtmlAgilityPack;
using System.Collections.Generic;
using Ramone.HyperMedia.Html;
using Ramone.HyperMedia;
using Ramone.Tests.Blog.Resources;


namespace Ramone.Tests.Blog.Codecs.Html
{
  public class PostCodec_Html : BaseCodec_Html<Resources.Post>
  {
    public override Resources.Post ReadFromHtml(HtmlDocument html, ReaderContext context)
    {
      HtmlNode doc = html.DocumentNode;

      List<Anchor> links = new List<Anchor>(doc.Anchors());

      Post post = new Post
      {
        Title = doc.SelectNodes(@".//*[@class=""post-title""]").First().InnerText,
        Text = doc.SelectNodes(@".//*[@class=""post-content""]").First().InnerText,
        Links = links
      };

      return post;
    }
  }
}
