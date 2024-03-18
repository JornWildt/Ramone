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
      Url1 = new OpenSearchUrl { RelationType = "tv", MediaType = "text/html", Template = "http://search.com?q={searchTerms}" };
      Url2 = new OpenSearchUrl { RelationType = "home", MediaType = "text/html", Template = "http://search.com?q={searchTerms}" };
      Url3 = new OpenSearchUrl { RelationType = "tv", MediaType = "application/atom+xml", Template = "http://search.com?q={searchTerms}" };
      Url4 = new OpenSearchUrl { RelationType = "home", MediaType = "application/atom+xml", Template = "http://search.com?q={searchTerms}" };
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
      ILinkTemplate l1a = Urls.Select(Url1.RelationType);
      ILinkTemplate l2a = Urls.Select(Url2.RelationType);
      ILinkTemplate l1b = Urls.Select(Url1.RelationType, "text/html");
      ILinkTemplate l2b = Urls.Select(Url2.RelationType, "text/html");
      ILinkTemplate l3 = Urls.Select(Url3.RelationType, "application/atom+xml");
      ILinkTemplate l4 = Urls.Select(Url4.RelationType, "application/atom+xml");

      // Assert
      Assert.IsNotNull(l1a);
      Assert.IsNotNull(l2a);
      Assert.IsNotNull(l1b);
      Assert.IsNotNull(l2b);
      Assert.IsNotNull(l3);
      Assert.IsNotNull(l4);
      Assert.That(l1a.Template, Is.EqualTo(Url1.Template));
      Assert.That(l2a.Template, Is.EqualTo(Url2.Template));
      Assert.That(l1b.Template, Is.EqualTo(Url1.Template));
      Assert.That(l2b.Template, Is.EqualTo(Url2.Template));
      Assert.That(l3.Template, Is.EqualTo(Url3.Template));
      Assert.That(l4.Template, Is.EqualTo(Url4.Template));
    }


    [Test]
    public void CanBindTemplate()
    {
      // Arrange
      ILinkTemplate template = new OpenSearchUrl
                                   {
                                     Template = "http://search.com/?q={terms}",
                                     MediaType = "application/atom+xml",
                                     RelationType = "results"
                                   };

      // Act
      Request request = Session.Bind(template, new { terms = "abc" });

      // Assert
      Assert.IsNotNull(request);
      Assert.That(request.Url.AbsoluteUri, Is.EqualTo("http://search.com/?q=abc"));
    }
  }
}
