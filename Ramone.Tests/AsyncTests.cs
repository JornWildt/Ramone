using NUnit.Framework;
using System;


namespace Ramone.Tests
{
  // Not many tests here - they are dispersed among the normal tests

  [TestFixture]
  public class AsyncTests : TestHelper
  {
    [Test]
    // As imple test to verify that we got something right (mostly while modeling the API)
    public void CanDoAsyncRequest()
    {
      // Arrange
      Request request = Session.Bind(DossierTemplate, new { id = 8 });
      bool ok = false;

      TestAsync(wh =>
      {
        // Act
        request.Async().Get(r => 
        { 
          ok = true; 
          wh.Set(); 
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
  }
}
