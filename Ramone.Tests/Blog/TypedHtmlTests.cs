using System.Collections.Generic;
using NUnit.Framework;
using Ramone.HyperMedia;
using Ramone.IO;


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
  /// 
  /// Compare this code to the code found in HtmlDocumentTests.cs and consider what is the most readable.
  /// </remarks>
  [TestFixture]
  public class TypedHtmlTests : BlogTestHelper
  {
    [Test]
    public void CanLoadBlog()
    {
      // Arrange
      Request blogRequest = Session.Bind(BlogRootPath);

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
      Request blogRequest = Session.Bind(BlogRootPath);

      // Act ...

      // - GET blog
      Resources.Blog blog = blogRequest.Get<Resources.Blog>().Body;

      // - Fetch author link
      ILink authorLink = blog.Links.Select("author");

      // - Follow author link and get author data
      Resources.Author author = authorLink.Follow(Session).Get<Resources.Author>().Body;

      // Assert ...
      Assert.IsNotNull(author);

      // - Check e-mail of author
      Assert.AreEqual("Pete Peterson", author.Name);
      Assert.AreEqual("pp@ramonerest.dk", author.EMail);
    }


    [Test]
    public void CanFollowAllAuthorsAndGetAllEMails()
    {
      // Arrange
      HashSet<string> foundEMails = new HashSet<string>();
      Request blogRequest = Session.Bind(BlogRootPath);

      // Act ...

      // - GET blog
      Resources.Blog blog = blogRequest.Get<Resources.Blog>().Body;

      foreach (Resources.Blog.Post post in blog.Posts)
      {
        // - GET post
        Resources.Post fullPost = post.Links.Select("self").Follow(Session).Get<Resources.Post>().Body;

        // - Follow author link
        Resources.Author author = fullPost.Links.Select("author").Follow(Session).Get<Resources.Author>().Body;

        // - Register e-mail
        foundEMails.Add(author.EMail);
      }

      // Assert ...
      Assert.AreEqual(2, foundEMails.Count);
      Assert.IsTrue(foundEMails.Contains("bb@ramonerest.dk"));
      Assert.IsTrue(foundEMails.Contains("cc@ramonerest.dk"));
    }


    [Test]
    public void CanAddNewBlogItemIncludingImage()
    {
      // Arrange
      Request blogRequest = Session.Bind(BlogRootPath);

      // Act ...

      // - GET blog
      Resources.Blog blog = blogRequest.Get<Resources.Blog>().Body;

      // - Follow "edit" link and GET form describing how to input
      Response<Resources.CreatePostDescriptor> createDescriptor
        = blog.Links.Select("edit").Follow(Session).Get<Resources.CreatePostDescriptor>();

      // - Extract "create" form
      IKeyValueForm form = createDescriptor.Body.Form;

      // - Populate form inputs
      Resources.CreatePostArgs args = new Resources.CreatePostArgs
      {
        Title = "New item",
        Text = "Yaj!",
        Image = new File("..\\..\\data1.gif", "image/gif")
      };
      form.Value(args);

      // - Submit the form
      Resources.Post createdPost = form.Bind().Submit<Resources.Post>().Created();

      // Assert ...
      Assert.IsNotNull(createdPost);
      Assert.AreEqual("New item", createdPost.Title);
      Assert.AreEqual("Yaj!", createdPost.Text);
    }
  }
}
