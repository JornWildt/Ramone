using System.Collections.Generic;
using NUnit.Framework;
using Ramone.HyperMedia;
using Ramone.MediaTypes.OpenSearch;


namespace Ramone.Tests.HyperMedia
{
  [TestFixture]
  public class LinkTemplateTests : TestHelper
  {
    OpenSearchUrl Url1;
    OpenSearchUrl Url2;
    OpenSearchUrl Url3;
    OpenSearchUrl Url4;
    List<OpenSearchUrl> Urls;
    OpenSearchDescription SearchDescription;


    protected override void SetUp()
    {
      base.SetUp();
      Url1 = new OpenSearchUrl { RelationshipType = "tv", MediaType = "text/html", Template = "http://search.com?q={searchTerms}" };
      Url2 = new OpenSearchUrl { RelationshipType = "home", MediaType = "text/html", Template = "http://search.com?q={searchTerms}" };
      Url3 = new OpenSearchUrl { RelationshipType = "tv", MediaType = "application/atom+xml", Template = "http://search.com?q={searchTerms}" };
      Url4 = new OpenSearchUrl { RelationshipType = "home", MediaType = "application/atom+xml", Template = "http://search.com?q={searchTerms}" };
      Urls = new List<OpenSearchUrl>();
      Urls.Add(Url1);
      Urls.Add(Url2);
      Urls.Add(Url3);
      Urls.Add(Url4);
      SearchDescription = new OpenSearchDescription { Urls = Urls };
    }


    [Test]
    public void CanSelectTemplateFromLinkList()
    {
      // Act
      ILinkTemplate l1a = Urls.Select(Url1.RelationshipType);
      ILinkTemplate l2a = Urls.Select(Url2.RelationshipType);
      ILinkTemplate l1b = Urls.Select(Url1.RelationshipType, "text/html");
      ILinkTemplate l2b = Urls.Select(Url2.RelationshipType, "text/html");
      ILinkTemplate l3 = Urls.Select(Url3.RelationshipType, "application/atom+xml");
      ILinkTemplate l4 = Urls.Select(Url4.RelationshipType, "application/atom+xml");

      // Assert
      Assert.IsNotNull(l1a);
      Assert.IsNotNull(l2a);
      Assert.IsNotNull(l1b);
      Assert.IsNotNull(l2b);
      Assert.IsNotNull(l3);
      Assert.IsNotNull(l4);
      Assert.AreEqual(Url1.Template, l1a.Template);
      Assert.AreEqual(Url2.Template, l2a.Template);
      Assert.AreEqual(Url1.Template, l1b.Template);
      Assert.AreEqual(Url2.Template, l2b.Template);
      Assert.AreEqual(Url3.Template, l3.Template);
      Assert.AreEqual(Url4.Template, l4.Template);
    }


    [Test]
    public void CanBindTemplate()
    {
      // Arrange
      ILinkTemplate template = new OpenSearchUrl
                                   {
                                     Template = "http://search.com/?q={terms}",
                                     MediaType = "application/atom+xml",
                                     RelationshipType = "results"
                                   };

      // Act
      RamoneRequest request = Session.Bind(template, new { terms = "abc" });

      // Assert
      Assert.IsNotNull(request);
      Assert.AreEqual("http://search.com/?q=abc", request.Url.AbsoluteUri);
    }
  }
}
