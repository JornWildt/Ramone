using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using NUnit.Framework;
using Ramone.HyperMedia;
using Ramone.MediaTypes.Html;


namespace Ramone.Tests.HyperMedia.Html
{
  [TestFixture]
  public class HtmlDocumentLinkTests : TestHelper
  {
    HtmlDocument HtmlDoc;
    //Uri BaseUrl = new Uri("http://example.com");

    string Html = @"
<html>
  <head>
    <link rel=""search""
          type=""application/opensearchdescription+xml"" 
          href=""http://example.com""
          title=""Content search"" />
    <link rel=""stylesheet"" 
          type=""text/css"" 
          href=""mystyle.css"" />
  </head>
  <body>
    <p>Hello World: <a href=""http://link1"" rel=""next"">Link no. 1</a></p>
    <a>A-Bomb!</a>
    <div id=""set1"">
      <p><a href=""http://link2"" rel=""up"">Link no. 2</a></p>
      <a>B-Bomb!</a>
    </div>
    <a href=""last-page.html"" rel=""last"">Next page</a>
  </body>
</html>";


    protected override void TestFixtureSetUp()
    {
      base.TestFixtureSetUp();
      HtmlDoc = new HtmlDocument();
      HtmlDoc.LoadHtml(Html);
    }


    [Test]
    public void CanExtractAnchorLinksFromHtmlDocument()
    {
      // Act
      List<Anchor> links = HtmlDoc.Anchors(BaseUrl).ToList();

      // Assert
      Assert.That(links.Count, Is.EqualTo(5));

      ILink l1 = links[0];
      Assert.That(l1.HRef.AbsoluteUri, Is.EqualTo("http://link1/"));
      Assert.That(l1.Title, Is.EqualTo("Link no. 1"));
      Assert.Contains("next", l1.RelationTypes.ToList());

      ILink l2 = links[2];
      Assert.That(l2.HRef.AbsoluteUri, Is.EqualTo("http://link2/"));
      Assert.That(l2.Title, Is.EqualTo("Link no. 2"));
      Assert.Contains("up", l2.RelationTypes.ToList());
    }


    [Test]
    public void CanExtractAnchorLinksFromHtmlNode()
    {
      // Arrange (get sub-node)
      HtmlNode node = HtmlDoc.DocumentNode.SelectNodes(@"//div[@id=""set1""]").First();

      // Act
      List<Anchor> links = node.Anchors(BaseUrl).ToList();

      // Assert
      Assert.That(links.Count, Is.EqualTo(2));
      ILink l1 = links[0];

      Assert.That(l1.HRef.AbsoluteUri, Is.EqualTo("http://link2/"));
      Assert.That(l1.Title, Is.EqualTo("Link no. 2"));
      Assert.Contains("up", l1.RelationTypes.ToList());
    }


    [Test]
    public void CanExtractAnchorLinkByRelationFromHtmlDocument()
    {
      // Act
      ILink link1 = HtmlDoc.Anchors(BaseUrl).Select("next");
      ILink link2 = HtmlDoc.Anchors(BaseUrl).Select("up");

      // Assert
      Assert.That(link1.HRef.AbsoluteUri, Is.EqualTo("http://link1/"));
      Assert.That(link1.Title, Is.EqualTo("Link no. 1"));
      Assert.Contains("next", link1.RelationTypes.ToList());

      Assert.That(link2.HRef.AbsoluteUri, Is.EqualTo("http://link2/"));
      Assert.That(link2.Title, Is.EqualTo("Link no. 2"));
      Assert.Contains("up", link2.RelationTypes.ToList());
    }


    [Test]
    public void CanExtractAnchorLinkByRelationFromHtmlNode()
    {
      // Arrange (get sub-node)
      HtmlNode node = HtmlDoc.DocumentNode.SelectNodes(@"//div[@id=""set1""]").First();

      // Act
      ILink link = node.Anchors(BaseUrl).Select("up");

      // Assert
      Assert.That(link.HRef.AbsoluteUri, Is.EqualTo("http://link2/"));
      Assert.That(link.Title, Is.EqualTo("Link no. 2"));
      Assert.Contains("up", link.RelationTypes.ToList());
    }


    [Test]
    public void WhenSelectingUnknownLinksItThrowsSelectFailed()
    {
      // Arrange (get sub-node)
      HtmlNode node = HtmlDoc.DocumentNode.SelectNodes(@"//div[@id=""set1""]").First();

      // Act + Assert
      AssertThrows<SelectFailedException>(() => node.Anchors(BaseUrl).Select("next"));
    }


    [Test]
    public void ItIncludesBaseUrlWhenCreatingRelativeAnchorLinks()
    {
      // Act
      ILink link1 = HtmlDoc.Anchors(BaseUrl).Select("last");

      // Assert
      Assert.That(link1.HRef.AbsoluteUri, Is.EqualTo(new Uri(BaseUrl, "last-page.html").AbsoluteUri));
      Assert.Contains("last", link1.RelationTypes.ToList());
    }


    [Test]
    public void WhenBaseUrlIsNullItHandlesAbsoluteAnchorLinks()
    {
      // Arrange
      string html = @"
<html>
  <body>
    <p>Hello World: <a href=""http://link1"" rel=""next"">Link no. 1</a></p>
  </body>
</html>";
      HtmlDocument doc = new HtmlDocument();
      doc.LoadHtml(html);

      // Act
      ILink l1 = doc.Anchors((Uri)null).Select("next");
      ILink l2 = doc.Anchors((Response)null).Select("next");
      ILink l3 = doc.DocumentNode.Anchors((Uri)null).Select("next");
      ILink l4 = doc.DocumentNode.Anchors((Response)null).Select("next");
      ILink l5 = doc.DocumentNode.SelectNodes("//*").Anchors((Uri)null).Select("next");
      ILink l6 = doc.DocumentNode.SelectNodes("//*").Anchors((Response)null).Select("next");
      ILink l7 = doc.DocumentNode.SelectNodes("//a").First().Anchor((Uri)null);
      ILink l8 = doc.DocumentNode.SelectNodes("//a").First().Anchor((Response)null);

      // Assert
      Assert.That(l1.HRef.AbsoluteUri, Is.EqualTo("http://link1/"));
      Assert.That(l2.HRef.AbsoluteUri, Is.EqualTo("http://link1/"));
      Assert.That(l3.HRef.AbsoluteUri, Is.EqualTo("http://link1/"));
      Assert.That(l4.HRef.AbsoluteUri, Is.EqualTo("http://link1/"));
      Assert.That(l5.HRef.AbsoluteUri, Is.EqualTo("http://link1/"));
      Assert.That(l6.HRef.AbsoluteUri, Is.EqualTo("http://link1/"));
      Assert.That(l7.HRef.AbsoluteUri, Is.EqualTo("http://link1/"));
      Assert.That(l8.HRef.AbsoluteUri, Is.EqualTo("http://link1/"));
    }


    [Test]
    public void CanExtractHeadLinksFromHtmlDocument()
    {
      // Act
      List<Link> links = HtmlDoc.Links(BaseUrl).ToList();

      // Assert
      Assert.That(links.Count, Is.EqualTo(2));

      ILink l1 = links[0];
      Assert.That(l1.HRef.AbsoluteUri, Is.EqualTo("http://example.com/"));
      Assert.That(l1.Title, Is.EqualTo("Content search"));
      Assert.Contains("search", l1.RelationTypes.ToList());
      Assert.That((string)l1.MediaType, Is.EqualTo("application/opensearchdescription+xml"));
    }


    [Test]
    public void CanExtractHeadLinksFromNode()
    {
      // Act
      List<Link> links = HtmlDoc.DocumentNode.SelectNodes("//head").First().Links(BaseUrl).ToList();

      // Assert
      Assert.That(links.Count, Is.EqualTo(2));

      ILink l1 = links[0];
      Assert.That(l1.HRef.AbsoluteUri, Is.EqualTo("http://example.com/"));
      Assert.That(l1.Title, Is.EqualTo("Content search"));
      Assert.Contains("search", l1.RelationTypes.ToList());
      Assert.That((string)l1.MediaType, Is.EqualTo("application/opensearchdescription+xml"));
    }


    [Test]
    public void CanExtractHeadLinksFromNodes()
    {
      // Act
      List<Link> links = HtmlDoc.DocumentNode.SelectNodes("//head").Links(BaseUrl).ToList();

      // Assert
      Assert.That(links.Count, Is.EqualTo(2));

      ILink l1 = links[0];
      Assert.That(l1.HRef.AbsoluteUri, Is.EqualTo("http://example.com/"));
      Assert.That(l1.Title, Is.EqualTo("Content search"));
      Assert.Contains("search", l1.RelationTypes.ToList());
      Assert.That((string)l1.MediaType, Is.EqualTo("application/opensearchdescription+xml"));
    }


    [Test]
    public void CanExtractHeadLinkFromNode()
    {
      // Act
      ILink link = HtmlDoc.DocumentNode.SelectNodes("//head/link").First().Link(BaseUrl);

      // Assert
      Assert.That(link.HRef.AbsoluteUri, Is.EqualTo("http://example.com/"));
      Assert.That(link.Title, Is.EqualTo("Content search"));
      Assert.Contains("search", link.RelationTypes.ToList());
      Assert.That((string)link.MediaType, Is.EqualTo("application/opensearchdescription+xml"));
    }


    [Test]
    public void ItIncludesBaseUrlWhenCreatingRelativeHeadLinks()
    {
      // Act
      List<Link> links = HtmlDoc.Links(BaseUrl).ToList();

      // Assert
      Assert.That(links.Count, Is.EqualTo(2));

      ILink l1 = links[1];
      Assert.That(l1.HRef, Is.EqualTo(new Uri(BaseUrl, "mystyle.css")));
      Assert.Contains("stylesheet", l1.RelationTypes.ToList());
    }


    [Test]
    public void WhenBaseUrlIsNullItHandlesAbsoluteHeadLinks()
    {
      // Arrange
      string html = @"
<html>
  <head>
    <link rel=""search""
          type=""application/opensearchdescription+xml"" 
          href=""http://example.com""
          title=""Content search"" />
  </head>
</html>";
      HtmlDocument doc = new HtmlDocument();
      doc.LoadHtml(html);

      // Act
      ILink l1 = doc.Links((Uri)null).Select("search");
      ILink l2 = doc.Links((Response)null).Select("search");
      ILink l3 = doc.DocumentNode.Links((Uri)null).Select("search");
      ILink l4 = doc.DocumentNode.Links((Response)null).Select("search");
      ILink l5 = doc.DocumentNode.SelectNodes("//*").Links((Uri)null).Select("search");
      ILink l6 = doc.DocumentNode.SelectNodes("//*").Links((Response)null).Select("search");
      ILink l7 = doc.DocumentNode.SelectNodes("//link").First().Link((Uri)null);
      ILink l8 = doc.DocumentNode.SelectNodes("//link").First().Link((Response)null);

      // Assert
      Assert.That(l1.HRef.AbsoluteUri, Is.EqualTo("http://example.com/"));
      Assert.That(l2.HRef.AbsoluteUri, Is.EqualTo("http://example.com/"));
      Assert.That(l3.HRef.AbsoluteUri, Is.EqualTo("http://example.com/"));
      Assert.That(l4.HRef.AbsoluteUri, Is.EqualTo("http://example.com/"));
      Assert.That(l5.HRef.AbsoluteUri, Is.EqualTo("http://example.com/"));
      Assert.That(l6.HRef.AbsoluteUri, Is.EqualTo("http://example.com/"));
      Assert.That(l7.HRef.AbsoluteUri, Is.EqualTo("http://example.com/"));
      Assert.That(l8.HRef.AbsoluteUri, Is.EqualTo("http://example.com/"));
    }
  }
}
