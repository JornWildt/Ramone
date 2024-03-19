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
      Assert.That(ok, Is.True);
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
              Assert.That(error, Is.Not.Null);
              Assert.That(error.Exception, Is.Not.Null);
              Assert.That(error.Response, Is.Not.Null);
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
      Assert.That(onErrorHandled, Is.True);
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
      Assert.That(onErrorHandled, Is.True);
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

      Assert.That(error, Is.Not.Null);
      Assert.That(error.Exception, Is.InstanceOf<InvalidOperationException>());
      Assert.That(error.Response, Is.Not.Null);
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
      Assert.That(gotOk, Is.False);
      Assert.That(gotError, Is.False);
      Assert.That(gotComplete, Is.True);
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
      Assert.That(gotOk, Is.True);
      Assert.That(gotError, Is.False);
      Assert.That(gotComplete, Is.True);
    }
  }
}
