using System;
using NUnit.Framework;
using Ramone.Tests.Common.CMS;


namespace Ramone.Tests
{
  [TestFixture]
  public class RequestBuilderTests : TestHelper
  {
    [Test]
    public void CanBindUri()
    {
      // Act
      Uri url = DossierTemplate.Bind(BaseUrl, new { id = 8 });

      // Assert
      Assert.AreEqual(new Uri(BaseUrl, CMSConstants.DossierPath.Replace("{id}", "8")), url);
    }


    [Test]
    public void CanBindRequest()
    {
      // Act
      RamoneRequest request = Session.Bind(DossierTemplate, new { id = 8 });

      // Assert
      Assert.AreEqual(new Uri(BaseUrl, CMSConstants.DossierPath.Replace("{id}", "8")), request.Url);
    }


    [Test]
    public void CanCreateRequestWithSessionFromUrl()
    {
      // Arrange
      Uri url = DossierTemplate.Bind(BaseUrl, new { id = 8 });

      // Act
      RamoneRequest request = Session.Request(url);

      // Assert
      Assert.AreEqual(new Uri(BaseUrl, CMSConstants.DossierPath.Replace("{id}", "8")), request.Url);
    }
  }
}