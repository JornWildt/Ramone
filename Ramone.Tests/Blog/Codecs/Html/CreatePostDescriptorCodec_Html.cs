using System;
using System.Linq;
using HtmlAgilityPack;
using Ramone.MediaTypes.Html;
using Ramone.Tests.Blog.Resources;


namespace Ramone.Tests.Blog.Codecs.Html
{
  public class CreatePostDescriptorCodec_Html : BaseCodec_Html<Resources.CreatePostDescriptor>
  {
    public override Resources.CreatePostDescriptor ReadFromHtml(HtmlDocument html, ReaderContext context)
    {
      HtmlNode doc = html.DocumentNode;

      Resources.CreatePostDescriptor descriptor = new CreatePostDescriptor
      {
        Form = doc.SelectNodes(@"//form").First().Form(context.Session, new Uri(context.Response.ResponseUri.GetLeftPart(UriPartial.Path)))
      };

      return descriptor;
    }
  }
}
