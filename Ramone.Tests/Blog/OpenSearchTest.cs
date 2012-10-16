using System.Linq;
using System.ServiceModel.Syndication;
using NUnit.Framework;
using Ramone.HyperMedia;
using Ramone.MediaTypes.OpenSearch;
using Ramone.MediaTypes;


namespace Ramone.Tests.Blog
{
  /// <summary>
  /// Test class illustrating the use of Link templates with Open Search.
  /// </summary>
  /// <remarks>The purpose of these tests is primarily to show how link templates can be
  /// used with Ramone. The first media-type that came to my mind for that purpose was Open Search
  /// and so it became a Open Search / Link Template demo.</remarks>
  [TestFixture]
  public class OpenSearchTests : BlogTestHelper
  {
    [Test]
    public void CanGetSearchLinkFromBlog()
    {
      // Arrange
      Request blogRequest = Session.Bind(BlogRootPath);

      // Act - get blog resource and select Open Search link
      using (var blog = blogRequest.Get<Resources.Blog>())
      {
        ILink searchLink = blog.Body.Links.Select("search", "application/opensearchdescription+xml");

        // Assert
        Assert.IsNotNull(searchLink);
      }
    }


    [Test]
    public void CanLoadSearchDescriptionAndGetResultUrl()
    {
      // Arrange
      ILink searchLink = GetSearchLink();

      // Act - follow Open Search link and get search description document. 
      // Ramone delivers codecs for Open Search.
      using (var response = searchLink.Follow(Session).Get<OpenSearchDescription>())
      {
        OpenSearchDescription search = response.Body;

        // Assert
        Assert.AreEqual("Blog Search", search.ShortName);
        Assert.AreEqual("Searching for blogs.", search.Description);
        Assert.AreEqual("jw@fjeldgruppen.dk", search.Contact);

        Assert.AreEqual(1, search.Urls.Count);
        ILinkTemplate l1 = search.Urls.Select("results");
        Assert.IsNotNull(l1);
      }
    }


    [Test]
    public void CanSearch()
    {
      // Arrange
      OpenSearchDescription description = GetSearchDescription();

      // Act
      ILinkTemplate searchTemplate = description.Urls[0];
      using (var response = Session.Bind(searchTemplate, new { searchTerms = "" }).Get<SyndicationFeed>())
      {
        SyndicationFeed result = response.Body;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Items.Count());
        SyndicationItem i1 = result.Items.First();
        Assert.AreEqual("Result 1", ((TextSyndicationContent)i1.Title).Text);
      }
    }


    ILink GetSearchLink()
    {
      Request blogRequest = Session.Bind(BlogRootPath);
      using (var blog = blogRequest.Get<Resources.Blog>())
      {
        ILink searchLink = blog.Body.Links.Select("search", "application/opensearchdescription+xml");
        return searchLink;
      }
    }


    OpenSearchDescription GetSearchDescription()
    {
      ILink searchLink = GetSearchLink();
      using (var search = searchLink.Follow(Session).Get<OpenSearchDescription>())
        return search.Body;
    }
  }
}
