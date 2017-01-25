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
    public void CanDoOptions_AsyncEvent()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsyncEvent()
          .OnError(error => Assert.Fail())
          .Options(response =>
          {
            // Assert
            Assert.AreEqual("2", response.Headers["X-ExtraHeader"]);
            wh.Set();
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
    public void CanDoOptionsWithBody_AsyncEvent()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsyncEvent()
          .OnError(error => Assert.Fail())
          .Options<string>(response =>
          {
            // Assert
            Assert.AreEqual("2", response.Headers["X-ExtraHeader"]);
            Assert.AreEqual("Yes", response.Body);
            wh.Set();
          });
      });
    }


    #region OPTIONS with empty/null callbacks

    [Test]
    public void CanOptionsAsyncEventWithoutHandler()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsyncEvent()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Options();
      });
    }


    [Test]
    public void CanOptionsAsyncEventWithoutHandler_Typed()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsyncEvent()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Options<string>();
      });
    }


    [Test]
    public void CanOptionsAsyncEventWithNullHandler()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsyncEvent()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Options(null);
      });
    }


    [Test]
    public void CanOptionsAsyncEventWithNullHandler_Typed()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        DossierReq.AsyncEvent()
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
