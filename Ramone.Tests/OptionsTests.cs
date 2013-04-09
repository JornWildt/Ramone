using NUnit.Framework;


namespace Ramone.Tests
{
  [TestFixture]
  public class OptionsTests : TestHelper
  {
    Request DossierReq;


    protected override void SetUp()
    {
      base.SetUp();
      DossierReq = Session.Bind(VerifiedMethodDossierTemplate, new { id = 8, method = "OPTIONS" });
    }


    [Test]
    public void CanDoOptions()
    {
      // Act
      using (Response response = DossierReq.Options())
      {
        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual("2", response.Headers["X-ExtraHeader"]);
      }
    }


    [Test]
    public void CanDoOptions_Async()
    {
      TestAsync(wh =>
      {
        // Act
        DossierReq.Async()
          .OnError(error => Assert.Fail())
          .OnComplete(() => wh.Set())
          .Options(response =>
          {
            // Assert
            Assert.AreEqual("2", response.Headers["X-ExtraHeader"]);
          });
      });
    }


    [Test]
    public void CanDoOptionsWithReturnedBody()
    {
      // Act
      using (Response<string> response1 = DossierReq.Options<string>())
      using (Response<string> response2 = DossierReq.Accept<string>().Options())
      using (Response response3 = DossierReq.Accept("text/plain").Options())
      {
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
    }


    [Test]
    public void CanDoOptionsWithBody_Async()
    {
      TestAsync(wh =>
      {
        // Act
        DossierReq.Async()
          .OnError(error => Assert.Fail())
          .OnComplete(() => wh.Set())
          .Options<string>(response =>
          {
            // Assert
            Assert.AreEqual("2", response.Headers["X-ExtraHeader"]);
            Assert.AreEqual("Yes", response.Body);
          });
      });
    }


    #region OPTIONS with empty/null callbacks

    [Test]
    public void CanOptionsAsyncWithoutHandler()
    {
      TestAsync(wh =>
      {
        // Act
        DossierReq.Async()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Options();
      });
    }


    [Test]
    public void CanOptionsAsyncWithoutHandler_Typed()
    {
      TestAsync(wh =>
      {
        // Act
        DossierReq.Async()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Options<string>();
      });
    }


    [Test]
    public void CanOptionsAsyncWithNullHandler()
    {
      TestAsync(wh =>
      {
        // Act
        DossierReq.Async()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Options(null);
      });
    }


    [Test]
    public void CanOptionsAsyncWithNullHandler_Typed()
    {
      TestAsync(wh =>
      {
        // Act
        DossierReq.Async()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Options<string>(null);
      });
    }

    #endregion
  }
}
