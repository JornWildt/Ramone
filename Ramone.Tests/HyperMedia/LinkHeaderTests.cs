using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Ramone.HyperMedia;


namespace Ramone.Tests.HyperMedia
{
  [TestFixture]
  public class LinkHeaderTests : TestHelper
  {
    [Test]
    public void CanReadLinksFromLinkHeader()
    {
      // Arrange
      Request request = Session.Bind(LinkHeaderTemplate);

      // Act
      using (Response response = request.Get())
      {
        // Assert
        List<WebLink> links = response.Links().ToList();
        Assert.That(links.Count, Is.EqualTo(2));
        Assert.That(links[0].HRef.AbsoluteUri, Is.EqualTo("http://example.com/TheBook/chapter2"));
        Assert.That(links[1].HRef.AbsoluteUri, Is.EqualTo("http://example.com/TheBook/chapter4"));
      }
    }


    [Test]
    public void WhenLinkHeaderIsEmptyOrNullItRetursEmptyList()
    {
      // Arrange
      Request request = Session.Bind(CatTemplate, new { name = "Alex" });

      // Act
      using (Response response = request.Get())
      {
        // Assert
        List<WebLink> links = response.Links().ToList();
        Assert.That(links, Is.Not.Null);
        Assert.That(links.Count, Is.EqualTo(0));
      }
    }
  }
}
