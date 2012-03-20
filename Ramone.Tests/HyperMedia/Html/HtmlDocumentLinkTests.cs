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
      Assert.AreEqual(4, links.Count);

      ILink l1 = links[0];
      Assert.AreEqual("http://link1", l1.HRef);
      Assert.AreEqual("Link no. 1", l1.Title);
      Assert.Contains("next", l1.RelationTypes.ToList());

      ILink l2 = links[2];
      Assert.AreEqual("http://link2", l2.HRef);
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

      Assert.AreEqual("http://link2", l1.HRef);
      Assert.AreEqual("Link no. 2", l1.Title);
      Assert.Contains("up", l1.RelationTypes.ToList());
    }


    [Test]
    public void CanExtractLinkByRelationFromHtmlDocument()
    {
      // Act
      ILink link1 = HtmlDoc.Anchors(BaseUrl).Select("next");
      ILink link2 = HtmlDoc.Anchors(BaseUrl).Select("up");

      // Assert
      Assert.AreEqual("http://link1", link1.HRef);
      Assert.AreEqual("Link no. 1", link1.Title);
      Assert.Contains("next", link1.RelationTypes.ToList());
    
      Assert.AreEqual("http://link2", link2.HRef);
      Assert.AreEqual("Link no. 2", link2.Title);
      Assert.Contains("up", link2.RelationTypes.ToList());
    }


    [Test]
    public void CanExtractLinkByRelationFromHtmlNode()
    {
      // Arrange (get sub-node)
      HtmlNode node = HtmlDoc.DocumentNode.SelectNodes(@"//div[@id=""set1""]").First();

      // Act
      ILink link1 = node.Anchors(BaseUrl).Select("next");
      ILink link2 = node.Anchors(BaseUrl).Select("up");

      // Assert
      Assert.IsNull(link1);

      Assert.AreEqual("http://link2", link2.HRef);
      Assert.AreEqual("Link no. 2", link2.Title);
      Assert.Contains("up", link2.RelationTypes.ToList());
    }


    [Test]
    public void CanExtractHeadLinksFromHtmlDocument()
    {
      // Act
      List<Link> links = HtmlDoc.Links(BaseUrl).ToList();

      // Assert
      Assert.AreEqual(2, links.Count);

      ILink l1 = links[0];
      Assert.AreEqual("http://example.com", l1.HRef);
      Assert.AreEqual("Content search", l1.Title);
      Assert.Contains("search", l1.RelationTypes.ToList());
      Assert.AreEqual("application/opensearchdescription+xml", l1.MediaType);
    }


    [Test]
    public void CanExtractLinksFromNode()
    {
      // Act
      List<Link> links = HtmlDoc.DocumentNode.SelectNodes("//head").First().Links(BaseUrl).ToList();

      // Assert
      Assert.AreEqual(2, links.Count);

      ILink l1 = links[0];
      Assert.AreEqual("http://example.com", l1.HRef);
      Assert.AreEqual("Content search", l1.Title);
      Assert.Contains("search", l1.RelationTypes.ToList());
      Assert.AreEqual("application/opensearchdescription+xml", l1.MediaType);
    }


    [Test]
    public void CanExtractLinksFromNodes()
    {
      // Act
      List<Link> links = HtmlDoc.DocumentNode.SelectNodes("//head").Links(BaseUrl).ToList();

      // Assert
      Assert.AreEqual(2, links.Count);

      ILink l1 = links[0];
      Assert.AreEqual("http://example.com", l1.HRef);
      Assert.AreEqual("Content search", l1.Title);
      Assert.Contains("search", l1.RelationTypes.ToList());
      Assert.AreEqual("application/opensearchdescription+xml", l1.MediaType);
    }


    [Test]
    public void CanExtractLinkFromNode()
    {
      // Act
      ILink link = HtmlDoc.DocumentNode.SelectNodes("//head/link").First().Link(BaseUrl);

      // Assert
      Assert.AreEqual("http://example.com", link.HRef);
      Assert.AreEqual("Content search", link.Title);
      Assert.Contains("search", link.RelationTypes.ToList());
      Assert.AreEqual("application/opensearchdescription+xml", link.MediaType);
    }
  }
}
