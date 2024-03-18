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
      IList<WebLink> links = WebLinkParser.ParseLinks(new Uri("http://example.com"), header);

      // Assert
      Assert.IsNotNull(links);
      Assert.That(links.Count, Is.EqualTo(2));

      ILink l1 = links[0];
      Assert.That(l1.HRef.AbsoluteUri, Is.EqualTo("http://example.com/TheBook/chapter2"));
      Assert.Contains("previous", l1.RelationTypes.ToList());
      Assert.That(l1.Title, Is.EqualTo("Previous chapter"));

      ILink l2 = links[1];
      Assert.That(l2.HRef.AbsoluteUri, Is.EqualTo("http://example.com/TheBook/chapter4"));
      Assert.Contains("next", l2.RelationTypes.ToList());
      Assert.That(l2.Title, Is.EqualTo("Next chapter"));
    }


    [Test]
    public void CanReadSingleWebLink()
    {
      // Arrange
      string header = @"<http://example.com/TheBook/chapter3>; rel=""previous""; title=""Previous chapter""";

      // Act
      IList<WebLink> links = WebLinkParser.ParseLinks(new Uri("http://example.com"), header);

      // Assert
      Assert.IsNotNull(links);
      Assert.That(links.Count, Is.EqualTo(1));

      ILink l1 = links[0];
      Assert.That(l1.HRef.AbsoluteUri, Is.EqualTo("http://example.com/TheBook/chapter3"));
      Assert.Contains("previous", l1.RelationTypes.ToList());
      Assert.That(l1.Title, Is.EqualTo("Previous chapter"));
    }


    [Test]
    public void CanReadSingleUrl()
    {
      // Arrange
      string header = @"<http://example.com/TheBook/chapter5>";

      // Act
      IList<WebLink> links = WebLinkParser.ParseLinks(new Uri("http://example.com"), header);

      // Assert
      Assert.IsNotNull(links);
      Assert.That(links.Count, Is.EqualTo(1));

      ILink l1 = links[0];
      Assert.That(l1.HRef.AbsoluteUri, Is.EqualTo("http://example.com/TheBook/chapter5"));
      Assert.That(l1.RelationTypes.Count(), Is.EqualTo(0));
      Assert.IsNull(l1.Title);
    }


    [Test]
    public void CanReadInternationalTitles()
    {
      // Arrange
      string header = @"<http://example.com/TheBook/chapter6>; rel=""previous""; title*=""UTF-8'de'N%c3%a4chstes%20Kapitel""";

      // Act
      IList<WebLink> links = WebLinkParser.ParseLinks(new Uri("http://example.com"), header);

      // Assert
      Assert.IsNotNull(links);
      Assert.That(links.Count, Is.EqualTo(1));

      ILink l1 = links[0];
      Assert.That(l1.HRef.AbsoluteUri, Is.EqualTo("http://example.com/TheBook/chapter6"));
      Assert.That(l1.Title, Is.EqualTo("Nächstes Kapitel"));
    }


    [Test]
    public void WhenBothNormalAndIntlTitleExistsItSelectsInternational()
    {
      // Arrange
      string header1 = @"<http://example.com/TheBook/chapter6>; rel=""previous""; title=""abc""; title*=""UTF-8'de'N%c3%a4chstes%20Kapitel""";
      string header2 = @"<http://example.com/TheBook/chapter6>; rel=""previous""; title*=""UTF-8'de'N%c3%a4chstes%20Kapitel""; title=""abc""";

      // Act
      IList<WebLink> links1 = WebLinkParser.ParseLinks(new Uri("http://example.com"), header1);
      IList<WebLink> links2 = WebLinkParser.ParseLinks(new Uri("http://example.com"), header2);

      // Assert
      Assert.IsNotNull(links1);
      Assert.IsNotNull(links2);
      Assert.That(links1.Count, Is.EqualTo(1));
      Assert.That(links2.Count, Is.EqualTo(1));

      ILink l1 = links1[0];
      Assert.That(l1.Title, Is.EqualTo("Nächstes Kapitel"));

      ILink l2 = links2[0];
      Assert.That(l2.Title, Is.EqualTo("Nächstes Kapitel"));
    }


    [Test]
    public void ItSelectsFirstTitleOnly()
    {
      // Arrange
      string header1 = @"<http://example.com/TheBook/chapter6>; rel=""previous""; title=""abc""; title=""123""";

      // Act
      IList<WebLink> links1 = WebLinkParser.ParseLinks(new Uri("http://example.com"), header1);

      // Assert
      Assert.IsNotNull(links1);
      Assert.That(links1.Count, Is.EqualTo(1));

      ILink l1 = links1[0];
      Assert.That(l1.Title, Is.EqualTo("abc"));
    }


    [Test]
    public void ItSelectsFirstRelOnly()
    {
      // Arrange
      string header1 = @"<http://example.com/TheBook/chapter6>; rel=""previous""; rel=""next""; title=""Abc""";

      // Act
      IList<WebLink> links1 = WebLinkParser.ParseLinks(new Uri("http://example.com"), header1);

      // Assert
      Assert.IsNotNull(links1);
      Assert.That(links1.Count, Is.EqualTo(1));

      ILink l1 = links1[0];
      Assert.Contains("previous", l1.RelationTypes.ToList());
    }


    [Test]
    public void CanReadTokenRels()
    {
      // Arrange
      string header1 = @"<http://example.com/TheBook/chapter6>; rel=next-chap.ter; title=""Abc""";

      // Act
      IList<WebLink> links1 = WebLinkParser.ParseLinks(new Uri("http://example.com"), header1);

      // Assert
      Assert.IsNotNull(links1);
      Assert.That(links1.Count, Is.EqualTo(1));

      ILink l1 = links1[0];
      Assert.Contains("next-chap.ter", l1.RelationTypes.ToList());
    }


    [Test]
    public void CanReadLinksWithMultipleRelValues()
    {
      // Arrange
      string header1 = @"<http://example.com/TheBook/chapter6>; rel=""next-chap.ter  prev   next""; title=""Abc""";

      // Act
      IList<WebLink> links1 = WebLinkParser.ParseLinks(new Uri("http://example.com"), header1);

      // Assert
      Assert.IsNotNull(links1);
      Assert.That(links1.Count, Is.EqualTo(1));

      ILink l1 = links1[0];
      Assert.That(l1.RelationTypes.Count(), Is.EqualTo(3));
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
      IList<WebLink> links1 = WebLinkParser.ParseLinks(new Uri("http://example.com"), header1);

      // Assert
      Assert.IsNotNull(links1);
      Assert.That(links1.Count, Is.EqualTo(2));

      ILink l1 = links1[0];
      Assert.That(l1.HRef.AbsoluteUri, Is.EqualTo("http://example.com/TheBook/chapter6"));
      Assert.That(l1.RelationTypes.Count(), Is.EqualTo(0));
      Assert.That(l1.Title, Is.EqualTo("Abc"));

      ILink l2 = links1[1];
      Assert.That(l2.HRef.AbsoluteUri, Is.EqualTo("http://example.com/TheBook/chapter1"));
      Assert.Contains("help", l2.RelationTypes.ToList());
      Assert.That(l2.Title, Is.EqualTo("Xyz"));
    }


    [Test]
    public void ItSkipsLinksWithSyntaxtErrors()
    {
      // Arrange
      string header1 = @";<http://example.com/TheBook/chapter6> rel=next-chap.ter; title=""Abc"",
  <http://example.com/TheBook/chapter1>; rel=""help""; title=""Xyz""";

      // Act
      IList<WebLink> links1 = WebLinkParser.ParseLinks(new Uri("http://example.com"), header1);

      // Assert
      Assert.IsNotNull(links1);
      Assert.That(links1.Count, Is.EqualTo(1));

      ILink l1 = links1[0];
      Assert.That(l1.HRef.AbsoluteUri, Is.EqualTo("http://example.com/TheBook/chapter1"));
      Assert.Contains("help", l1.RelationTypes.ToList());
      Assert.That(l1.Title, Is.EqualTo("Xyz"));
    }
  }
}
