using System.Linq;
using HtmlAgilityPack;
using Ramone.HyperMedia.Html;
using Ramone.Tests.Blog.Resources;


namespace Ramone.Tests.Blog.Codecs.Html
{
  public class CreatePostDescriptorCodec_Html : BaseCodec_Html<Resources.CreatePostDescriptor>
  {
    public override Resources.CreatePostDescriptor ReadFromHtml(HtmlDocument html)
    {
      HtmlNode doc = html.DocumentNode;

      Resources.CreatePostDescriptor descriptor = new CreatePostDescriptor
      {
        Form = doc.SelectNodes(@"//form").First().Form()
      };

      return descriptor;
    }
  }
}
