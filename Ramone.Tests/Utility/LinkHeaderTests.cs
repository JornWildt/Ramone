using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Ramone.HyperMedia;
using Ramone.Utility;


namespace Ramone.Tests.Utility
{
  [TestFixture]
  public class LinkHeaderTests : TestHelper
  {

    [Test]
    public void CanReadMultipleWebLinks()
    {
      // Arrange
      string header = @"<http://example.com/TheBook/chapter2>; rel=""previous""; title=""Previous chapter"", 
  <http://example.com/TheBook/chapter4>; rel=""next""; title=""Next chapter""";

      // Act
      IList<IParameterizedLink> links = WebLinkParser.ParseLinks(new Uri("http://example.com"), header);

      // Assert
      Assert.IsNotNull(links);
      Assert.AreEqual(2, links.Count);

      ILink l1 = links[0];
      Assert.AreEqual("http://example.com/TheBook/chapter2", l1.HRef.AbsoluteUri);
      Assert.Contains("previous", l1.RelationTypes.ToList());
      Assert.AreEqual("Previous chapter", l1.Title);

      ILink l2 = links[1];
      Assert.AreEqual("http://example.com/TheBook/chapter4", l2.HRef.AbsoluteUri);
      Assert.Contains("next", l2.RelationTypes.ToList());
      Assert.AreEqual("Next chapter", l2.Title);
    }


    [Test]
    public void CanReadSingleWebLink()
    {
      // Arrange
      string header = @"<http://example.com/TheBook/chapter3>; rel=""previous""; title=""Previous chapter""";

      // Act
      IList<IParameterizedLink> links = WebLinkParser.ParseLinks(new Uri("http://example.com"), header);

      // Assert
      Assert.IsNotNull(links);
      Assert.AreEqual(1, links.Count);

      ILink l1 = links[0];
      Assert.AreEqual("http://example.com/TheBook/chapter3", l1.HRef.AbsoluteUri);
      Assert.Contains("previous", l1.RelationTypes.ToList());
      Assert.AreEqual("Previous chapter", l1.Title);
    }


    [Test]
    public void CanReadSingleUrl()
    {
      // Arrange
      string header = @"<http://example.com/TheBook/chapter5>";

      // Act
      IList<IParameterizedLink> links = WebLinkParser.ParseLinks(new Uri("http://example.com"), header);

      // Assert
      Assert.IsNotNull(links);
      Assert.AreEqual(1, links.Count);

      ILink l1 = links[0];
      Assert.AreEqual("http://example.com/TheBook/chapter5", l1.HRef.AbsoluteUri);
      Assert.AreEqual(0, l1.RelationTypes.Count());
      Assert.IsNull(l1.Title);
    }


    [Test]
    public void CanReadInternationalTitles()
    {
      // Arrange
      string header = @"<http://example.com/TheBook/chapter6>; rel=""previous""; title*=""UTF-8'de'N%c3%a4chstes%20Kapitel""";

      // Act
      IList<IParameterizedLink> links = WebLinkParser.ParseLinks(new Uri("http://example.com"), header);

      // Assert
      Assert.IsNotNull(links);
      Assert.AreEqual(1, links.Count);

      ILink l1 = links[0];
      Assert.AreEqual("http://example.com/TheBook/chapter6", l1.HRef.AbsoluteUri);
      Assert.AreEqual("Nächstes Kapitel", l1.Title);
    }


    [Test]
    public void WhenBothNormalAndIntlTitleExistsItSelectsInternational()
    {
      // Arrange
      string header1 = @"<http://example.com/TheBook/chapter6>; rel=""previous""; title=""abc""; title*=""UTF-8'de'N%c3%a4chstes%20Kapitel""";
      string header2 = @"<http://example.com/TheBook/chapter6>; rel=""previous""; title*=""UTF-8'de'N%c3%a4chstes%20Kapitel""; title=""abc""";

      // Act
      IList<IParameterizedLink> links1 = WebLinkParser.ParseLinks(new Uri("http://example.com"), header1);
      IList<IParameterizedLink> links2 = WebLinkParser.ParseLinks(new Uri("http://example.com"), header2);

      // Assert
      Assert.IsNotNull(links1);
      Assert.IsNotNull(links2);
      Assert.AreEqual(1, links1.Count);
      Assert.AreEqual(1, links2.Count);

      ILink l1 = links1[0];
      Assert.AreEqual("Nächstes Kapitel", l1.Title);

      ILink l2 = links2[0];
      Assert.AreEqual("Nächstes Kapitel", l2.Title);
    }


    [Test]
    public void ItSelectsFirstTitleOnly()
    {
      // Arrange
      string header1 = @"<http://example.com/TheBook/chapter6>; rel=""previous""; title=""abc""; title=""123""";

      // Act
      IList<IParameterizedLink> links1 = WebLinkParser.ParseLinks(new Uri("http://example.com"), header1);

      // Assert
      Assert.IsNotNull(links1);
      Assert.AreEqual(1, links1.Count);

      ILink l1 = links1[0];
      Assert.AreEqual("abc", l1.Title);
    }


    [Test]
    public void ItSelectsFirstRelOnly()
    {
      // Arrange
      string header1 = @"<http://example.com/TheBook/chapter6>; rel=""previous""; rel=""next""; title=""Abc""";

      // Act
      IList<IParameterizedLink> links1 = WebLinkParser.ParseLinks(new Uri("http://example.com"), header1);

      // Assert
      Assert.IsNotNull(links1);
      Assert.AreEqual(1, links1.Count);

      ILink l1 = links1[0];
      Assert.Contains("previous", l1.RelationTypes.ToList());
    }


    [Test]
    public void CanReadTokenRels()
    {
      // Arrange
      string header1 = @"<http://example.com/TheBook/chapter6>; rel=next-chap.ter; title=""Abc""";

      // Act
      IList<IParameterizedLink> links1 = WebLinkParser.ParseLinks(new Uri("http://example.com"), header1);

      // Assert
      Assert.IsNotNull(links1);
      Assert.AreEqual(1, links1.Count);

      ILink l1 = links1[0];
      Assert.Contains("next-chap.ter", l1.RelationTypes.ToList());
    }


    [Test]
    public void CanReadLinksWithMultipleRelValues()
    {
      // Arrange
      string header1 = @"<http://example.com/TheBook/chapter6>; rel=""next-chap.ter  prev   next""; title=""Abc""";

      // Act
      IList<IParameterizedLink> links1 = WebLinkParser.ParseLinks(new Uri("http://example.com"), header1);

      // Assert
      Assert.IsNotNull(links1);
      Assert.AreEqual(1, links1.Count);

      ILink l1 = links1[0];
      Assert.AreEqual(3, l1.RelationTypes.Count());
      Assert.Contains("next-chap.ter", l1.RelationTypes.ToList());
      Assert.Contains("prev", l1.RelationTypes.ToList());
      Assert.Contains("next", l1.RelationTypes.ToList());
    }


    [Test]
    public void ItSkipsAttributesWithSyntaxErrors()
    {
      // Arrange
      string header1 = @"<http://example.com/TheBook/chapter6>; x rel=next-chap.ter; title=""Abc"",
  <http://example.com/TheBook/chapter1>; rel=""help""; title=""Xyz""";

      // Act
      IList<IParameterizedLink> links1 = WebLinkParser.ParseLinks(new Uri("http://example.com"), header1);

      // Assert
      Assert.IsNotNull(links1);
      Assert.AreEqual(2, links1.Count);

      ILink l1 = links1[0];
      Assert.AreEqual("http://example.com/TheBook/chapter6", l1.HRef.AbsoluteUri);
      Assert.AreEqual(0, l1.RelationTypes.Count());
      Assert.AreEqual("Abc", l1.Title);

      ILink l2 = links1[1];
      Assert.AreEqual("http://example.com/TheBook/chapter1", l2.HRef.AbsoluteUri);
      Assert.Contains("help", l2.RelationTypes.ToList());
      Assert.AreEqual("Xyz", l2.Title);
    }


    [Test]
    public void ItSkipsLinksWithSyntaxtErrors()
    {
      // Arrange
      string header1 = @";<http://example.com/TheBook/chapter6> rel=next-chap.ter; title=""Abc"",
  <http://example.com/TheBook/chapter1>; rel=""help""; title=""Xyz""";

      // Act
      IList<IParameterizedLink> links1 = WebLinkParser.ParseLinks(new Uri("http://example.com"), header1);

      // Assert
      Assert.IsNotNull(links1);
      Assert.AreEqual(1, links1.Count);

      ILink l1 = links1[0];
      Assert.AreEqual("http://example.com/TheBook/chapter1", l1.HRef.AbsoluteUri);
      Assert.Contains("help", l1.RelationTypes.ToList());
      Assert.AreEqual("Xyz", l1.Title);
    }
  }
}
