using NUnit.Framework;
using System;


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
  }
}
