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
    public void CannotUseTheNormalOperations()
    {
      Request request = Session.Bind(DossierTemplate, new { id = 8 });
      AssertThrows<InvalidOperationException>(() => request.Async().Get());
      AssertThrows<InvalidOperationException>(() => request.Async().Get<object>());
      AssertThrows<InvalidOperationException>(() => request.Async().Post());
      AssertThrows<InvalidOperationException>(() => request.Async().Post<object>());
      AssertThrows<InvalidOperationException>(() => request.Async().Post(new object()));
      AssertThrows<InvalidOperationException>(() => request.Async().Post<object>(new object()));
      AssertThrows<InvalidOperationException>(() => request.Async().Put());
      AssertThrows<InvalidOperationException>(() => request.Async().Put<object>());
      AssertThrows<InvalidOperationException>(() => request.Async().Put(new object()));
      AssertThrows<InvalidOperationException>(() => request.Async().Put<object>(new object()));
      AssertThrows<InvalidOperationException>(() => request.Async().Delete());
      AssertThrows<InvalidOperationException>(() => request.Async().Delete<object>());
      AssertThrows<InvalidOperationException>(() => request.Async().Patch());
      AssertThrows<InvalidOperationException>(() => request.Async().Patch<object>());
      AssertThrows<InvalidOperationException>(() => request.Async().Patch(new object()));
      AssertThrows<InvalidOperationException>(() => request.Async().Patch<object>(new object()));
      AssertThrows<InvalidOperationException>(() => request.Async().Options());
      AssertThrows<InvalidOperationException>(() => request.Async().Options<object>());
      AssertThrows<InvalidOperationException>(() => request.Async().Head());
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
  }
}
