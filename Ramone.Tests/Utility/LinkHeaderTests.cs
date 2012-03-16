using System.Collections.Generic;
using NUnit.Framework;
using Ramone.HyperMedia;
using Ramone.Utility;
using System.Text;
using System.Web;


namespace Ramone.Tests.Utility
{
  [TestFixture]
  public class LinkHeaderTests : TestHelper
  {
    string WebLinks1 = @"<http://example.com/TheBook/chapter2>; rel=""previous""; title=""Previous chapter"", 
  <http://example.com/TheBook/chapter4>; rel=""next""; title=""Next chapter""";

    string WebLinks2 = @"<http://example.com/TheBook/chapter3>; rel=""previous""; title=""Previous chapter""";

    string WebLinks3 = @"<http://example.com/TheBook/chapter5>";

    string WebLinks4 = @"<http://example.com/TheBook/chapter6>; rel=""previous""; title*=""UTF-8'de'N%c3%a4chstes%20Kapitel""";


    [Test]
    public void CanReadMultipleWebLinks()
    {
      // Act
      IList<ILink> links = WebLinkParser.ParseLinks(WebLinks1);

      // Assert
      Assert.IsNotNull(links);
      Assert.AreEqual(2, links.Count);

      ILink l1 = links[0];
      Assert.AreEqual("http://example.com/TheBook/chapter2", l1.HRef);
      Assert.AreEqual("previous", l1.RelationshipType);
      Assert.AreEqual("Previous chapter", l1.Title);

      ILink l2 = links[1];
      Assert.AreEqual("http://example.com/TheBook/chapter4", l2.HRef);
      Assert.AreEqual("next", l2.RelationshipType);
      Assert.AreEqual("Next chapter", l2.Title);
    }


    [Test]
    public void CanReadSingleWebLink()
    {
      // Act
      IList<ILink> links = WebLinkParser.ParseLinks(WebLinks2);

      // Assert
      Assert.IsNotNull(links);
      Assert.AreEqual(1, links.Count);

      ILink l1 = links[0];
      Assert.AreEqual("http://example.com/TheBook/chapter3", l1.HRef);
      Assert.AreEqual("previous", l1.RelationshipType);
      Assert.AreEqual("Previous chapter", l1.Title);
    }


    [Test]
    public void CanReadSingleUrl()
    {
      // Act
      IList<ILink> links = WebLinkParser.ParseLinks(WebLinks3);

      // Assert
      Assert.IsNotNull(links);
      Assert.AreEqual(1, links.Count);

      ILink l1 = links[0];
      Assert.AreEqual("http://example.com/TheBook/chapter5", l1.HRef);
      Assert.IsNull(l1.RelationshipType);
      Assert.IsNull(l1.Title);
    }


    [Test]
    public void CanReadInternationalTitles()
    {
      // Act
      IList<ILink> links = WebLinkParser.ParseLinks(WebLinks4);

      // Assert
      Assert.IsNotNull(links);
      Assert.AreEqual(1, links.Count);

      ILink l1 = links[0];
      Assert.AreEqual("http://example.com/TheBook/chapter6", l1.HRef);
      Assert.AreEqual("Nächstes Kapitel", l1.Title);
    }
  }
}
