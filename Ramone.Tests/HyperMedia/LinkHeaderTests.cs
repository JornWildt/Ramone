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
      Response response = request.Get();

      // Assert
      List<WebLink> links = response.Links().ToList();
      Assert.AreEqual(2, links.Count);
      Assert.AreEqual("http://example.com/TheBook/chapter2", links[0].HRef.AbsoluteUri);
      Assert.AreEqual("http://example.com/TheBook/chapter4", links[1].HRef.AbsoluteUri);
    }


    [Test]
    public void WhenLinkHeaderIsEmptyOrNullItRetursEmptyList()
    {
      // Arrange
      Request request = Session.Bind(CatTemplate, new { name = "Alex" });

      // Act
      Response response = request.Get();

      // Assert
      List<WebLink> links = response.Links().ToList();
      Assert.IsNotNull(links);
      Assert.AreEqual(0, links.Count);
    }
  }
}
