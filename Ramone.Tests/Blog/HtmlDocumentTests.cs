using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using NUnit.Framework;
using Ramone.HyperMedia;
using Ramone.IO;
using Ramone.MediaTypes.Html;


namespace Ramone.Tests.Blog
{
  /// <summary>
  /// These test shows various blog scenarios using the generic "text/html" media-type.
  /// </summary>
  /// <remarks>
  /// The use of text/html as HtmlDocument requires no extra codec registrations, but instead
  /// it requires you to work with XPath expressions and dynamical access to data. This can be
  /// fine with simple one-off scenarios but as soon as you start copying the xpath strings and
  /// access patterns all over the code it becomes a maintaince nightmare. Then it is time to
  /// create codecs specific for the service and work with a type-safe access to the data.
  /// 
  /// Compare this code to the code found in TypedHtmlTests.cs and consider what is the most readable.
  /// </remarks>
  [TestFixture]
  public class HtmlDocumentTests : BlogTestHelper
  {
    [Test]
    public void CanLoadBlog()
    {
      // Arrange
      Request blogRequest = Session.Bind(BlogRootPath);

      // Act ...
      using (var blog = blogRequest.Get<HtmlDocument>())
      {
        // Extract post entries from the HTML
        HtmlNodeCollection posts = blog.Body.DocumentNode.SelectNodes(@"//div[@class=""post""]");

        // Assert ...
        Assert.That(posts.Count, Is.EqualTo(2));

        // - Check content of first post
        HtmlNode post1 = posts[0];
        Assert.That(post1, Is.Not.Null);
        HtmlNode post1Title = post1.SelectNodes(@".//*[@class=""post-title""]").First();
        HtmlNode post1Content = post1.SelectNodes(@".//*[@class=""post-content""]").First();
        Assert.That(post1Title.InnerText, Is.EqualTo("Hot summer"));
        Assert.That(post1Content.InnerText, Is.EqualTo("It is a hot summer this year."));
      }
    }


    [Test]
    public void CanFollowAuthorLinkFromBlog()
    {
      // Arrange
      Request blogRequest = Session.Bind(BlogRootPath);

      // Act ...

      // - GET blog
      using (var blog = blogRequest.Get<HtmlDocument>())
      {
        // - Select first HTML anchor node with rel="author" as a anchor link
        //   This uses HtmlDocument specific extension methods to convert from anchor to ILink
        ILink authorLink = blog.Body.DocumentNode.SelectNodes(@"//a[@rel=""author""]").First().Anchor(blog);

        // - Follow author link and get HTML document representing the author
        using (var author = authorLink.Follow(Session).Get<HtmlDocument>())
        {
          // Assert ...
          Assert.That(author, Is.Not.Null);

          // - Check e-mail of author
          HtmlNode email = author.Body.DocumentNode.SelectNodes(@"//a[@rel=""email""]").First();
          Assert.That(email.Attributes["href"].Value, Is.EqualTo("pp@ramonerest.dk"));
        }
      }
    }


    [Test]
    public void CanFollowAllAuthorsAndGetAllEMails()
    {
      // Arrange
      HashSet<string> foundEMails = new HashSet<string>();
      Request blogRequest = Session.Bind(BlogRootPath);

      // Act ...

      // - GET blog
      using (var blog = blogRequest.Get<HtmlDocument>())
      {
        // - Extract "post" nodes
        HtmlNodeCollection posts = blog.Body.DocumentNode.SelectNodes(@"//*[@class=""post""]");

        foreach (HtmlNode listPost in posts)
        {
          // - Extract post link, follow and GET the post
          ILink postLink = listPost.SelectNodes(@".//a[@rel=""self""]").First().Anchor(blog);
          using (var postItem = postLink.Follow(Session).Get<HtmlDocument>())
          {
            // - Extract author link from post
            ILink authorLink = postItem.Body.DocumentNode.SelectNodes(@".//a[@rel=""author""]").First().Anchor(blog);

            // - Follow author link and get HTML document representing the author
            using (var author = authorLink.Follow(Session).Get<HtmlDocument>())
            {
              // - Get e-mail of author
              HtmlNode email = author.Body.DocumentNode.SelectNodes(@"//a[@rel=""email""]").First();

              foundEMails.Add(email.Attributes["href"].Value);
            }
          }
        }

        // Assert ...
        Assert.That(foundEMails.Count, Is.EqualTo(2));
        Assert.That(foundEMails.Contains("bb@ramonerest.dk"), Is.True);
        Assert.That(foundEMails.Contains("cc@ramonerest.dk"), Is.True);
      }
    }


    [Test]
    public void CanAddNewBlogItemIncludingImage()
    {
      // Arrange
      Request blogRequest = Session.Bind(BlogRootPath);

      // Act ...

      // - GET blog
      using (var blog = blogRequest.Get<HtmlDocument>())
      {
        // - Extract "edit" anchor
        ILink editLink = blog.Body.DocumentNode.SelectNodes(@"//a[@rel=""edit""]").First().Anchor(blog);

        // - GET form describing how to input
        using (Response<HtmlDocument> createDescriptor = editLink.Follow(Session).Get<HtmlDocument>())
        {
          // - Extract "create" form
          IKeyValueForm form = createDescriptor.Body.DocumentNode.SelectNodes(@"//form[@id=""create""]").First().Form(createDescriptor);

          // - Populate form inputs
          IFile file = new File("..\\..\\data1.gif", "image/gif");
          form.Value("Title", "New item");
          form.Value("Text", "Yaj!");
          form.Value("Image", file);

          // - Submit the form
          using (var createdBlogItem = form.Bind().Submit<HtmlDocument>())
          {
            // Assert ...
            Assert.That(createdBlogItem, Is.Not.Null);
            HtmlNode postTitle = createdBlogItem.Body.DocumentNode.SelectNodes(@"//*[@class=""post-title""]").First();
            HtmlNode postContent = createdBlogItem.Body.DocumentNode.SelectNodes(@"//*[@class=""post-content""]").First();
            Assert.That(postTitle.InnerText, Is.EqualTo("New item"));
            Assert.That(postContent.InnerText, Is.EqualTo("Yaj!"));
          }
        }
      }
    }
  }
}
