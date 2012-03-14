using NUnit.Framework;
using Ramone.HyperMedia;
using Ramone.MediaTypes.OpenSearch;


namespace Ramone.Tests.Blog
{
  [TestFixture]
  public class OpenSearchTests : BlogTestHelper
  {
    [Test]
    public void CanGetSearchLinkFromBlog()
    {
      // Arrange
      RamoneRequest blogRequest = Session.Bind(BlogRootPath);

      // Act
      Resources.Blog blog = blogRequest.Get<Resources.Blog>().Body;

      // Assert
      ILink searchLink = blog.Links.Link("search", "application/opensearchdescription+xml");
      Assert.IsNotNull(searchLink);
    }


    [Test]
    public void CanLoadDescription()
    {
      // Arrange
      RamoneRequest blogRequest = Session.Bind(BlogRootPath);
      Resources.Blog blog = blogRequest.Get<Resources.Blog>().Body;
      ILink searchLink = blog.Links.Link("search", "application/opensearchdescription+xml");

      // Act
      OpenSearchDescription search = searchLink.Follow(Session).Get<OpenSearchDescription>().Body;

      // Assert
      Assert.AreEqual("Blog Search", search.ShortName);
      Assert.AreEqual(1, search.Urls.Count);
      ILinkTemplate l1 = search.Urls[0];
      Assert.IsNotNull(l1);
    }
  }
}
