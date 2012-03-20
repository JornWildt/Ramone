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
      Assert.AreEqual(5, links.Count);

      ILink l1 = links[0];
      Assert.AreEqual("http://link1/", l1.HRef.AbsoluteUri);
      Assert.AreEqual("Link no. 1", l1.Title);
      Assert.Contains("next", l1.RelationTypes.ToList());

      ILink l2 = links[2];
      Assert.AreEqual("http://link2/", l2.HRef.AbsoluteUri);
      Assert.AreEqual("Link no. 2", l2.Title);
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
      Assert.AreEqual(2, links.Count);
      ILink l1 = links[0];

      Assert.AreEqual("http://link2/", l1.HRef.AbsoluteUri);
      Assert.AreEqual("Link no. 2", l1.Title);
      Assert.Contains("up", l1.RelationTypes.ToList());
    }


    [Test]
    public void CanExtractAnchorLinkByRelationFromHtmlDocument()
    {
      // Act
      ILink link1 = HtmlDoc.Anchors(BaseUrl).Select("next");
      ILink link2 = HtmlDoc.Anchors(BaseUrl).Select("up");

      // Assert
      Assert.AreEqual("http://link1/", link1.HRef.AbsoluteUri);
      Assert.AreEqual("Link no. 1", link1.Title);
      Assert.Contains("next", link1.RelationTypes.ToList());

      Assert.AreEqual("http://link2/", link2.HRef.AbsoluteUri);
      Assert.AreEqual("Link no. 2", link2.Title);
      Assert.Contains("up", link2.RelationTypes.ToList());
    }


    [Test]
    public void CanExtractAnchorLinkByRelationFromHtmlNode()
    {
      // Arrange (get sub-node)
      HtmlNode node = HtmlDoc.DocumentNode.SelectNodes(@"//div[@id=""set1""]").First();

      // Act
      ILink link1 = node.Anchors(BaseUrl).Select("next");
      ILink link2 = node.Anchors(BaseUrl).Select("up");

      // Assert
      Assert.IsNull(link1);

      Assert.AreEqual("http://link2/", link2.HRef.AbsoluteUri);
      Assert.AreEqual("Link no. 2", link2.Title);
      Assert.Contains("up", link2.RelationTypes.ToList());
    }


    [Test]
    public void ItIncludesBaseUrlWhenCreatingRelativeAnchorLinks()
    {
      // Act
      ILink link1 = HtmlDoc.Anchors(BaseUrl).Select("last");

      // Assert
      Assert.AreEqual(new Uri(BaseUrl, "last-page.html"), link1.HRef);
      Assert.Contains("last", link1.RelationTypes.ToList());
    }


    [Test]
    public void CanExtractHeadLinksFromHtmlDocument()
    {
      // Act
      List<Link> links = HtmlDoc.Links(BaseUrl).ToList();

      // Assert
      Assert.AreEqual(2, links.Count);

      ILink l1 = links[0];
      Assert.AreEqual("http://example.com/", l1.HRef.AbsoluteUri);
      Assert.AreEqual("Content search", l1.Title);
      Assert.Contains("search", l1.RelationTypes.ToList());
      Assert.AreEqual("application/opensearchdescription+xml", l1.MediaType);
    }


    [Test]
    public void CanExtractHeadLinksFromNode()
    {
      // Act
      List<Link> links = HtmlDoc.DocumentNode.SelectNodes("//head").First().Links(BaseUrl).ToList();

      // Assert
      Assert.AreEqual(2, links.Count);

      ILink l1 = links[0];
      Assert.AreEqual("http://example.com/", l1.HRef.AbsoluteUri);
      Assert.AreEqual("Content search", l1.Title);
      Assert.Contains("search", l1.RelationTypes.ToList());
      Assert.AreEqual("application/opensearchdescription+xml", l1.MediaType);
    }


    [Test]
    public void CanExtractHeadLinksFromNodes()
    {
      // Act
      List<Link> links = HtmlDoc.DocumentNode.SelectNodes("//head").Links(BaseUrl).ToList();

      // Assert
      Assert.AreEqual(2, links.Count);

      ILink l1 = links[0];
      Assert.AreEqual("http://example.com/", l1.HRef.AbsoluteUri);
      Assert.AreEqual("Content search", l1.Title);
      Assert.Contains("search", l1.RelationTypes.ToList());
      Assert.AreEqual("application/opensearchdescription+xml", l1.MediaType);
    }


    [Test]
    public void CanExtractHeadLinkFromNode()
    {
      // Act
      ILink link = HtmlDoc.DocumentNode.SelectNodes("//head/link").First().Link(BaseUrl);

      // Assert
      Assert.AreEqual("http://example.com/", link.HRef.AbsoluteUri);
      Assert.AreEqual("Content search", link.Title);
      Assert.Contains("search", link.RelationTypes.ToList());
      Assert.AreEqual("application/opensearchdescription+xml", link.MediaType);
    }


    [Test]
    public void ItIncludesBaseUrlWhenCreatingRelativeHeadLinks()
    {
      // Act
      List<Link> links = HtmlDoc.Links(BaseUrl).ToList();

      // Assert
      Assert.AreEqual(2, links.Count);

      ILink l1 = links[1];
      Assert.AreEqual(new Uri(BaseUrl, "mystyle.css"), l1.HRef);
      Assert.Contains("stylesheet", l1.RelationTypes.ToList());
    }
  }
}
