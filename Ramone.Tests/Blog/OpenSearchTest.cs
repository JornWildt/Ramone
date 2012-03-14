using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Ramone.HyperMedia;

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

    }
  }
}
