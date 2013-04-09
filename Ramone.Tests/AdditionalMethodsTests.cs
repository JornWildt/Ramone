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
      using (Response response = dossierReq.Head())
      {
        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual("1", response.Headers["X-ExtraHeader"]);
      }
    }


    [Test]
    public void CanDoHead_Async()
    {
      // Arrange
      Request dossierReq = Session.Bind(DossierTemplate, new { id = 8 });

      TestAsync(wh =>
      {
        // Act
        dossierReq.Async()
          .OnError(error => Assert.Fail())
          .OnComplete(() => wh.Set())
          .Head(response =>
          {
            // Assert
            Assert.AreEqual("1", response.Headers["X-ExtraHeader"]);
          });
      });
    }


    [Test]
    public void CanDoAsyncHeadWithEmptyHandler()
    {
      // Arrange
      Request dossierReq = Session.Bind(VerifiedMethodDossierTemplate, new { id = 8, method = "HEAD" });

      TestAsync(wh =>
      {
        // Act
        dossierReq.Async()
          .OnError(error => Assert.Fail())
          .OnComplete(() => wh.Set())
          .Head();
      });
    }


    [Test]
    public void CanDoAsyncHeadWithNullHandler()
    {
      // Arrange
      Request dossierReq = Session.Bind(VerifiedMethodDossierTemplate, new { id = 8, method = "HEAD" });

      TestAsync(wh =>
      {
        // Act
        dossierReq.Async()
          .OnError(error => Assert.Fail())
          .OnComplete(() => wh.Set())
          .Head(null);
      });
    }
  }
}
