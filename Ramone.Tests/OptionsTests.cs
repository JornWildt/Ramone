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
        Assert.That(response, Is.Not.Null);
        Assert.That(response.Headers["X-ExtraHeader"], Is.EqualTo("2"));
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
            Assert.That(response.Headers["X-ExtraHeader"], Is.EqualTo("2"));
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
        Assert.That(response1, Is.Not.Null);
        Assert.That(response2, Is.Not.Null);
        Assert.That(response3, Is.Not.Null);
        Assert.That(response1.Headers["X-ExtraHeader"], Is.EqualTo("2"));
        Assert.That(response2.Headers["X-ExtraHeader"], Is.EqualTo("2"));
        Assert.That(response3.Headers["X-ExtraHeader"], Is.EqualTo("2"));
        Assert.That(response1.Body, Is.EqualTo("Yes"));
        Assert.That(response2.Body, Is.EqualTo("Yes"));
        Assert.That(response3.Decode<string>(), Is.EqualTo("Yes"));
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
            Assert.That(response.Headers["X-ExtraHeader"], Is.EqualTo("2"));
            Assert.That(response.Body, Is.EqualTo("Yes"));
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
