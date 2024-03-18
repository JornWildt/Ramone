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
        Assert.That(search.ShortName, Is.EqualTo("Blog Search"));
        Assert.That(search.Description, Is.EqualTo("Searching for blogs."));
        Assert.That(search.Contact, Is.EqualTo("jw@fjeldgruppen.dk"));

        Assert.That(search.Urls.Count, Is.EqualTo(1));
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
        Assert.That(result.Items.Count(), Is.EqualTo(1));
        SyndicationItem i1 = result.Items.First();
        Assert.That(((TextSyndicationContent)i1.Title).Text, Is.EqualTo("Result 1"));
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
