using NUnit.Framework;
using System;


namespace Ramone.Tests
{
  [TestFixture]
  public class AdditionalMethodsTests : TestHelper
  {
    [Test]
    public void CanDoHead()
    {
      // Arrange
      RamoneRequest dossierReq = Session.Bind(DossierTemplate, new { id = 8 });

      // Act
      Resource response = dossierReq.Head();

      // Assert
      Assert.IsNotNull(response);
      Assert.AreEqual("1", response.Headers["X-ExtraHeader"]);
    }


    [Test]
    public void CanDoOptions()
    {
      // Arrange
      RamoneRequest dossierReq = Session.Bind(DossierTemplate, new { id = 8 });

      // Act
      Resource response = dossierReq.Options();

      // Assert
      Assert.IsNotNull(response);
      Assert.AreEqual("2", response.Headers["X-ExtraHeader"]);
    }


    [Test]
    public void CanDoOptionsWithBody()
    {
      // Arrange
      RamoneRequest dossierReq = Session.Bind(DossierTemplate, new { id = 8 });

      // Act
      Resource<string> response1 = dossierReq.Options<string>();
      Resource<string> response2 = dossierReq.Accept<string>().Options();
      Resource response3 = dossierReq.Options("text/plain");

      // Assert
      Assert.IsNotNull(response1);
      Assert.IsNotNull(response2);
      Assert.IsNotNull(response3);
      Assert.AreEqual("2", response1.Headers["X-ExtraHeader"]);
      Assert.AreEqual("2", response2.Headers["X-ExtraHeader"]);
      Assert.AreEqual("2", response3.Headers["X-ExtraHeader"]);
      Assert.AreEqual("Yes", response1.Body);
      Assert.AreEqual("Yes", response2.Body);
      Assert.AreEqual("Yes", response3.Decode<string>());
    }


    [Test]
    public void CanDoOptionsWithStarPath()
    {
      // Not sure if this actually sends the correct "OPTIONS * HTTP/1.1" ...

      // Arrange
      RamoneRequest dossierReq = Session.Request(new Uri(Session.BaseUri, "*"));

      // Assert
      Assert.AreEqual(Session.BaseUri + "*", dossierReq.Url.AbsoluteUri);
    }
  }
}
