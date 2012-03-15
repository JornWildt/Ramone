using System.Linq;
using NUnit.Framework;
using Ramone.HyperMedia;
using Ramone.MediaTypes.OpenSearch;
using System.ServiceModel.Syndication;


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
    public void CanLoadSearchDescription()
    {
      // Arrange
      ILink searchLink = GetSearchLink();

      // Act
      OpenSearchDescription search = searchLink.Follow(Session).Get<OpenSearchDescription>().Body;

      // Assert
      Assert.AreEqual("Blog Search", search.ShortName);
      Assert.AreEqual("Searching for blogs.", search.Description);
      Assert.AreEqual("jw@fjeldgruppen.dk", search.Contact);
      Assert.AreEqual(1, search.Urls.Count);
      ILinkTemplate l1 = search.Urls[0];
      Assert.IsNotNull(l1);
    }


    [Test]
    public void CanSearch()
    {
      // Arrange
      OpenSearchDescription description = GetSearchDescription();

      // Act
      ILinkTemplate searchTemplate = description.Urls[0];
      SyndicationFeed result = searchTemplate.Bind(Session, new { searchTerms = "" }).Get<SyndicationFeed>().Body;

      // Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(1, result.Items.Count());
      SyndicationItem i1 = result.Items.First();
      Assert.AreEqual("Result 1", ((TextSyndicationContent)i1.Title).Text);
    }


    ILink GetSearchLink()
    {
      RamoneRequest blogRequest = Session.Bind(BlogRootPath);
      Resources.Blog blog = blogRequest.Get<Resources.Blog>().Body;
      ILink searchLink = blog.Links.Link("search", "application/opensearchdescription+xml");
      return searchLink;
    }


    OpenSearchDescription GetSearchDescription()
    {
      ILink searchLink = GetSearchLink();
      OpenSearchDescription search = searchLink.Follow(Session).Get<OpenSearchDescription>().Body;
      return search;
    }
  }
}
