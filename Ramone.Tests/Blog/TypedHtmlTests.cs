using NUnit.Framework;
using Ramone.HyperMedia;


namespace Ramone.Tests.Blog
{
  /// <summary>
  /// These test shows various blog scenarios using a typed approach the "text/html" media-type.
  /// </summary>
  /// <remarks>
  /// The use of text/html in a typed way requires registration of codecs that translates the HTML into
  /// typed values. How the codecs does that is irrelevant to the use of the typed values and thus we
  /// split parsing of the resource representation from the higher level "business" use of the data.
  /// This separation of concerns makes the code using the API much simpler, easier to read and easier
  /// to maintain.
  /// </remarks>
  [TestFixture]
  public class TypedHtmlTests : BlogTestHelper
  {
    protected override void TestFixtureSetUp()
    {
      base.TestFixtureSetUp();
      TestService.CodecManager.AddCodec<Resources.Blog, Codecs.Html.BlogCodec_Html>(MediaType.TextHtml);
      TestService.CodecManager.AddCodec<Resources.Author, Codecs.Html.AuthorCodec_Html>(MediaType.TextHtml);
    }


    [Test]
    public void CanLoadBlog()
    {
      // Arrange
      RamoneRequest blogRequest = Session.Bind(BlogRootPath);

      // Act
      Resources.Blog blog = blogRequest.Get<Resources.Blog>().Body;

      // Assert ...
      Assert.IsNotNull(blog);
      Assert.AreEqual("A mixed blog", blog.Title);
      Assert.IsNotNull(blog.Posts);
      Assert.AreEqual(2, blog.Posts.Count);

      // - Check content of first post
      Resources.Blog.Post post1 = blog.Posts[0];
      Assert.IsNotNull(post1);
      Assert.AreEqual("Hot summer", post1.Title);
      Assert.AreEqual("It is a hot summer this year.", post1.Text);
    }


    [Test]
    public void CanFollowAuthorLinkFromBlog()
    {
      // Arrange
      RamoneRequest blogRequest = Session.Bind(BlogRootPath);

      // Act ...

      // - GET blog
      Resources.Blog blog = blogRequest.Get<Resources.Blog>().Body;

      // - Fetch author link
      ILink authorLink = blog.Links.Link("author");

      // - Follow author link and get author data
      Resources.Author author = authorLink.Follow(Session).Get<Resources.Author>().Body;

      // Assert ...
      Assert.IsNotNull(author);

      // - Check e-mail of author
      Assert.AreEqual("Pete Peterson", author.Name);
      Assert.AreEqual("pp@ramonerest.dk", author.EMail);
    }

  }
}
