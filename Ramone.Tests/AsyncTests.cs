using NUnit.Framework;
using System;
using Ramone.Tests.Common;


namespace Ramone.Tests
{
  // Not many tests here - they are dispersed among the normal tests

  [TestFixture]
  public class AsyncTests : TestHelper
  {
    [Test]
    // As simple test to verify that we got something right (mostly while modeling the API)
    public void CanDoAsyncRequest()
    {
      // Arrange
      Request request = Session.Bind(DossierTemplate, new { id = 8 });
      bool ok = false;

      TestAsync(wh =>
      {
        // Act
        request.Async()
               .OnComplete(() => wh.Set())
               .Get(r => 
                    { 
                      ok = true; 
                    });
      });

      // Assert
      Assert.IsTrue(ok);
    }


    [Test]
    public void WhenExceptionIsThrownItIsPassedToErrorHandler()
    {
      // Arrange
      Request request = Session.Bind("/unknown-url");

      TestAsync(wh =>
        {
          request.Async().OnError(error =>
            {
              Assert.IsNotNull(error);
              Assert.IsNotNull(error.Exception);
              Assert.IsNotNull(error.Response);
              wh.Set();
            }).Get(r => { });
        });
    }


    [Test]
    public void WhenExceptionIsThrownItAlsoCallsOnComplete()
    {
      // Arrange
      Request request = Session.Bind("/unknown-url");
      bool onErrorHandled = false;

      TestAsync(wh =>
      {
        request.Async().OnError(error =>
        {
          onErrorHandled = true;
        }).OnComplete(() =>
        {
          Assert.IsTrue(onErrorHandled);
          wh.Set();
        }).Get(r => { });
      });
    }


    [Test]
    public void WhenExceptionIsThrownInCallbackItCallsErrorHandlerWithRequestAsWellAsCompleteHandler()
    {
      Request request = Session.Bind(DossierTemplate, new { id = 8 });
      AsyncError error = null;

      TestAsync(wh =>
      {
        // Act
        request.Async()
               .OnError(e => error = e)
               .OnComplete(() => wh.Set())
               .Get(r =>
               {
                 throw new InvalidOperationException();
               });
      });

      Assert.IsNotNull(error);
      Assert.IsInstanceOf<InvalidOperationException>(error.Exception);
      Assert.IsNotNull(error.Response);
    }


    [Test]
    public void CanCancelAsyncRequest()
    {
      // Arrange
      Request request = Session.Bind(Constants.SlowPath);
      bool gotOk = false;
      bool gotError = false;
      bool gotComplete = false;

      TestAsync(wh =>
      {
        // Act
        request.Async()
               .OnError(err => gotError = true)
               .OnComplete(() => { gotComplete = true; wh.Set(); })
               .Get(r => { gotOk = true; });

        request.CancelAsync();
      });

      // Assert
      Assert.IsFalse(gotOk);
      Assert.IsFalse(gotError);
      Assert.IsTrue(gotComplete);
    }


    [Test]
    public void ItIsSafeToCancelClosedAsyncRequest()
    {
      // Arrange
      Request request = Session.Bind(Constants.SlowPath);
      bool gotOk = false;
      bool gotError = false;
      bool gotComplete = false;

      TestAsync(wh =>
      {
        request.Async()
               .OnError(err => gotError = true)
               .OnComplete(() => { gotComplete = true; wh.Set(); })
               .Get(r => { gotOk = true; });
      });

      // Act
      request.CancelAsync();

      // Assert
      Assert.IsTrue(gotOk);
      Assert.IsFalse(gotError);
      Assert.IsTrue(gotComplete);
    }
  }
}
