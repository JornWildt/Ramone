using NUnit.Framework;
using System;
using Ramone.Tests.Common;


namespace Ramone.Tests
{
  // Not many tests here - they are dispersed among the normal tests

  [TestFixture]
  public class AsyncEventTests : TestHelper
  {
    [Test]
    // As simple test to verify that we got something right (mostly while modeling the API)
    public void CanDoAsyncEventRequest()
    {
      // Arrange
      Request request = Session.Bind(DossierTemplate, new { id = 8 });
      bool ok = false;

      TestAsyncEvent(wh =>
      {
        // Act
        request.AsyncEvent()
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

      TestAsyncEvent(wh =>
        {
          request.AsyncEvent().OnError(error =>
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

      // Act
      TestAsyncEvent(wh =>
      {
        request.AsyncEvent().OnError(error =>
        {
          onErrorHandled = true;
          wh.Set();
        }).OnComplete(() =>
        {
          wh.Set();
        }).Get(r => { });
      });

      // Assert
      Assert.IsTrue(onErrorHandled);
    }


    [Test]
    public void ItCallsOnErrorWhenRequestingUnknownService()
    {
      // Arrange
      Request req = new Request("http://unknown.very-unlikely-weqeqex2-hostname.dk");
      bool onErrorHandled = false;

      // Act
      TestAsyncEvent(wh =>
      {
        req.AsJson().AsyncEvent()
          .OnError(error =>
          {
            onErrorHandled = true;
            wh.Set();
          })
          .OnComplete(() =>
          {
            wh.Set();
          }).Post(new { a = 1 });
      });

      // Assert
      Assert.IsTrue(onErrorHandled);
    }


    [Test]
    public void WhenExceptionIsThrownInCallbackItCallsErrorHandlerWithRequestAsWellAsCompleteHandler()
    {
      Request request = Session.Bind(DossierTemplate, new { id = 8 });
      AsyncEventError error = null;

      TestAsyncEvent(wh =>
      {
        // Act
        request.AsyncEvent()
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
    public void CanCancelAsyncEventRequest()
    {
      // Arrange
      Request request = Session.Bind(Constants.SlowPath);
      bool gotOk = false;
      bool gotError = false;
      bool gotComplete = false;

      TestAsyncEvent(wh =>
      {
        // Act
        request.AsyncEvent()
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
    public void ItIsSafeToCancelClosedAsyncEventRequest()
    {
      // Arrange
      Request request = Session.Bind(Constants.SlowPath);
      bool gotOk = false;
      bool gotError = false;
      bool gotComplete = false;

      TestAsyncEvent(wh =>
      {
        request.AsyncEvent()
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
