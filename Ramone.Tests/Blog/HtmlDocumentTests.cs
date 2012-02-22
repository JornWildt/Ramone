using System.Linq;
using HtmlAgilityPack;
using NUnit.Framework;
using Ramone.HyperMedia;
using Ramone.HyperMedia.Html;


namespace Ramone.Tests.Blog
{
  /// <summary>
  /// These test shows interesting blog scenarios using the generic "text/html" media-type.
  /// </summary>
  /// <remarks>
  /// The use of text/html as HtmlDocument requires no extra codec registrations, but instead
  /// it requires you to work with XPath expressions and dynamical access to data. This can be
  /// fine with simple one-off scenarios but as soon as you start copying the xpath strings and
  /// access patterns all over the code it becomes a maintaince nightmare. Then it is time to
  /// create codecs specific for the service and work with a type-safe access to the data.
  /// </remarks>
  [TestFixture]
  public class HtmlDocumentTests : BlogTestHelper
  {
    [Test]
    public void CanLoadBlog()
    {
      // Arrange
      RamoneRequest blogRequest = Session.Bind(BlogRootPath);

      // Act ...
      HtmlDocument blog = blogRequest.Get<HtmlDocument>().Body;

      // Extract post entries from the HTML
      HtmlNodeCollection posts = blog.DocumentNode.SelectNodes(@"//div[@class=""post""]");

      // Assert ...
      Assert.AreEqual(2, posts.Count);

      // - Check content of first post
      HtmlNode post1 = posts[0];
      Assert.IsNotNull(post1);
      HtmlNode post1Title = post1.SelectNodes(@".//*[@class=""post-title""]").First();
      HtmlNode post1Content = post1.SelectNodes(@".//*[@class=""post-content""]").First();
      Assert.AreEqual("Blog Item No. 5", post1Title.InnerText);
      Assert.AreEqual("Blah blah blah ..:", post1Content.InnerText);
    }


    [Test]
    public void CanFollowAuthorLinkFromBlog()
    {
      // Arrange
      RamoneRequest blogRequest = Session.Bind(BlogRootPath);

      // Act ...

      // - Fetch blog
      HtmlDocument blog = blogRequest.Get<HtmlDocument>().Body;

      // - Select first HTML anchor node with rel="author" as a anchor link
      //   This uses HtmlDocument specific extension methods to convert from anchor to ILink
      ILink authorLink = blog.DocumentNode.SelectNodes(@"//a[@rel=""author""]").First().Anchor();

      // - Follow author link and get HTML document representing the author
      HtmlDocument author = authorLink.Follow(Session).Get<HtmlDocument>().Body;

      // Assert ...
      Assert.IsNotNull(author);

      // - Check e-mail of author
      HtmlNode email = author.DocumentNode.SelectNodes(@"//a[@rel=""email""]").First();
      Assert.AreEqual("pete3@mail.dk", email.Attributes["href"].Value);
    }
  }
}
