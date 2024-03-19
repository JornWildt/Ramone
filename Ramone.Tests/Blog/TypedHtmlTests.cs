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
      using (var response = blogRequest.Get<Resources.Blog>())
      {
        Resources.Blog blog = response.Body;

        // Assert ...
        Assert.That(blog, Is.Not.Null);
        Assert.That(blog.Title, Is.EqualTo("A mixed blog"));
        Assert.That(blog.Posts, Is.Not.Null);
        Assert.That(blog.Posts.Count, Is.EqualTo(2));

        // - Check content of first post
        Resources.Blog.Post post1 = blog.Posts[0];
        Assert.That(post1, Is.Not.Null);
        Assert.That(post1.Title, Is.EqualTo("Hot summer"));
        Assert.That(post1.Text, Is.EqualTo("It is a hot summer this year."));
      }
    }


    [Test]
    public void CanFollowAuthorLinkFromBlog()
    {
      // Arrange
      Request blogRequest = Session.Bind(BlogRootPath);

      // Act ...

      // - GET blog
      using (var response = blogRequest.Get<Resources.Blog>())
      {
        Resources.Blog blog = response.Body;

        // - Fetch author link
        ILink authorLink = blog.Links.Select("author");

        // - Follow author link and get author data
        using (var author = authorLink.Follow(Session).Get<Resources.Author>())
        {
          // Assert ...
          Assert.That(author.Body, Is.Not.Null);

          // - Check e-mail of author
          Assert.That(author.Body.Name, Is.EqualTo("Pete Peterson"));
          Assert.That(author.Body.EMail, Is.EqualTo("pp@ramonerest.dk"));
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
      using (var r = blogRequest.Get<Resources.Blog>())
      {
        Resources.Blog blog = r.Body;
        foreach (Resources.Blog.Post post in blog.Posts)
        {
          // - GET post
          using (var fullPost = post.Links.Select("self").Follow(Session).Get<Resources.Post>())
          {
            // - Follow author link
            using (var author = fullPost.Body.Links.Select("author").Follow(Session).Get<Resources.Author>())
            {
              // - Register e-mail
              foundEMails.Add(author.Body.EMail);
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
      using (var r = blogRequest.Get<Resources.Blog>())
      {
        Resources.Blog blog = r.Body;

        // - Follow "edit" link and GET form describing how to input
        using (Response<Resources.CreatePostDescriptor> createDescriptor
               = blog.Links.Select("edit").Follow(Session).Get<Resources.CreatePostDescriptor>())
        {
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
          using (var r2 = form.Bind().Submit<Resources.Post>())
          {
            Resources.Post createdPost = r2.Body;

            // Assert ...
            Assert.That(createdPost, Is.Not.Null);
            Assert.That(createdPost.Title, Is.EqualTo("New item"));
            Assert.That(createdPost.Text, Is.EqualTo("Yaj!"));
          }
        }
      }
    }
  }
}
