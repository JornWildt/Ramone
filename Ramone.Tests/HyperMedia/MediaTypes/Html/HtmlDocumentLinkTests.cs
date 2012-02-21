using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using HtmlAgilityPack;
using Ramone.HyperMedia;
using Ramone.HyperMedia.Html;


namespace Ramone.Tests.HyperMedia.MediaTypes.Html
{
  [TestFixture]
  public class HtmlDocumentLinkTests : TestHelper
  {
    HtmlDocument HtmlDoc;

    string Html = @"
<html>
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
    public void CanExtractLinksFromHtmlDocument()
    {
      // Act
      List<ILink> links = HtmlDoc.Links().ToList();

      // Assert
      Assert.AreEqual(4, links.Count);

      ILink l1 = links[0];
      Assert.AreEqual("http://link1", l1.HRef);
      Assert.AreEqual("Link no. 1", l1.Title);
      Assert.AreEqual("next", l1.RelationshipType);

      ILink l2 = links[2];
      Assert.AreEqual("http://link2", l2.HRef);
      Assert.AreEqual("Link no. 2", l2.Title);
      Assert.AreEqual("up", l2.RelationshipType);
    }


    [Test]
    public void CanExtractLinksFromHtmlNode()
    {
      // Arrange (get sub-node)
      HtmlNode node = HtmlDoc.DocumentNode.SelectNodes(@"//div[@id=""set1""]").First();

      // Act
      List<ILink> links = node.Links().ToList();

      // Assert
      Assert.AreEqual(2, links.Count);
      ILink l1 = links[0];

      Assert.AreEqual("http://link2", l1.HRef);
      Assert.AreEqual("Link no. 2", l1.Title);
      Assert.AreEqual("up", l1.RelationshipType);
    }


    [Test]
    public void CanExtractLinkByRelationFromHtmlDocument()
    {
      // Act
      ILink link1 = HtmlDoc.Link("next");
      ILink link2 = HtmlDoc.Link("up");

      // Assert
      Assert.AreEqual("http://link1", link1.HRef);
      Assert.AreEqual("Link no. 1", link1.Title);
      Assert.AreEqual("next", link1.RelationshipType);
    
      Assert.AreEqual("http://link2", link2.HRef);
      Assert.AreEqual("Link no. 2", link2.Title);
      Assert.AreEqual("up", link2.RelationshipType);
    }


    [Test]
    public void CanExtractLinkByRelationFromHtmlNode()
    {
      // Arrange (get sub-node)
      HtmlNode node = HtmlDoc.DocumentNode.SelectNodes(@"//div[@id=""set1""]").First();

      // Act
      ILink link1 = node.Link("next");
      ILink link2 = node.Link("up");

      // Assert
      Assert.IsNull(link1);

      Assert.AreEqual("http://link2", link2.HRef);
      Assert.AreEqual("Link no. 2", link2.Title);
      Assert.AreEqual("up", link2.RelationshipType);
    }
  }
}
