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
      Request dossierReq = Session.Bind(DossierTemplate, new { id = 8 });

      // Act
      Response response = dossierReq.Head();

      // Assert
      Assert.IsNotNull(response);
      Assert.AreEqual("1", response.Headers["X-ExtraHeader"]);
    }


    [Test]
    public void CanDoOptions()
    {
      // Arrange
      Request dossierReq = Session.Bind(DossierTemplate, new { id = 8 });

      // Act
      Response response = dossierReq.Options();

      // Assert
      Assert.IsNotNull(response);
      Assert.AreEqual("2", response.Headers["X-ExtraHeader"]);
    }


    [Test]
    public void CanDoOptionsWithBody()
    {
      // Arrange
      Request dossierReq = Session.Bind(DossierTemplate, new { id = 8 });

      // Act
      Response<string> response1 = dossierReq.Options<string>();
      Response<string> response2 = dossierReq.Accept<string>().Options();
      Response response3 = dossierReq.Accept("text/plain").Options();

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
      Request dossierReq = Session.Request(new Uri(Session.BaseUri, "*"));

      // Assert
      Assert.AreEqual(Session.BaseUri + "*", dossierReq.Url.AbsoluteUri);
    }
  }
}
