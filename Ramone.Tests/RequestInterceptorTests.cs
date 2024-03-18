using System.Threading.Tasks;
using NUnit.Framework;
using Ramone.MediaTypes.JsonPatch;
using Ramone.Tests.Common;


namespace Ramone.Tests
{
  [TestFixture]
  public class RequestInterceptorTests : TestHelper
  {
    [Test]
    public void InterceptorMethodsAreCalledOnce()
    {
      // Arrange
      var interceptor = new RegisterInterceptor();
      Session.RequestInterceptors.Add(interceptor);

      var request = Session.Bind(VerifiedMethodDossierTemplate, new { method = "POST", id = 8 }).AsJson();

      // Act
      using (request.Post(new { x = 0 }))
      {
        // Assert
        Assert.That(interceptor.MethodSet_Called, Is.EqualTo(1));
        Assert.That(interceptor.Headersready_Called, Is.EqualTo(1));
        Assert.That(interceptor.DataSent_Called, Is.EqualTo(1));
      }
    }


    [Test]
    public void InterceptorMethodsAreCalledOnceWithNoBody()
    {
      // Arrange
      var interceptor = new RegisterInterceptor();
      Session.RequestInterceptors.Add(interceptor);

      var request = Session.Bind(VerifiedMethodDossierTemplate, new { method = "POST", id = 8 }).AsJson();

      // Act
      using (request.Post())
      {
        // Assert
        Assert.That(interceptor.MethodSet_Called, Is.EqualTo(1));
        Assert.That(interceptor.Headersready_Called, Is.EqualTo(1));
        Assert.That(interceptor.DataSent_Called, Is.EqualTo(1));
      }
    }


    [Test]
    public void InterceptorMethodsAreCalledOnceWithGet()
    {
      // Arrange
      var interceptor = new RegisterInterceptor();
      Session.RequestInterceptors.Add(interceptor);

      var request = Session.Bind(VerifiedMethodDossierTemplate, new { method = "GET", id = 8 });

      // Act
      using (request.Get())
      {
        // Assert
        Assert.That(interceptor.MethodSet_Called, Is.EqualTo(1));
        Assert.That(interceptor.Headersready_Called, Is.EqualTo(1));
        Assert.That(interceptor.DataSent_Called, Is.EqualTo(1));
      }
    }


    [Test]
    public async Task InterceptorMethodsAreCalledOnce_Async()
    {
      // Arrange
      var interceptor = new RegisterInterceptor();
      Session.RequestInterceptors.Add(interceptor);

      var request = Session.Bind(VerifiedMethodDossierTemplate, new { method = "POST", id = 8 }).AsJson();

      // Act
      using (await request.Async().Post(new { x = 0 }))
      {
        // Assert
        Assert.That(interceptor.MethodSet_Called, Is.EqualTo(1));
        Assert.That(interceptor.Headersready_Called, Is.EqualTo(1));
        Assert.That(interceptor.DataSent_Called, Is.EqualTo(1));
      }
    }


    [Test]
    public async Task InterceptorMethodsAreCalledOnceWithNoBody_Async()
    {
      // Arrange
      var interceptor = new RegisterInterceptor();
      Session.RequestInterceptors.Add(interceptor);

      var request = Session.Bind(VerifiedMethodDossierTemplate, new { method = "POST", id = 8 }).AsJson();

      // Act
      using (await request.Async().Post())
      {
        // Assert
        Assert.That(interceptor.MethodSet_Called, Is.EqualTo(1));
        Assert.That(interceptor.Headersready_Called, Is.EqualTo(1));
        Assert.That(interceptor.DataSent_Called, Is.EqualTo(1));
      }
    }


    [Test]
    public async Task InterceptorMethodsAreCalledOnceWithGet_Async()
    {
      // Arrange
      var interceptor = new RegisterInterceptor();
      Session.RequestInterceptors.Add(interceptor);

      var request = Session.Bind(VerifiedMethodDossierTemplate, new { method = "GET", id = 8 });

      // Act
      using (await request.Async().Get())
      {
        // Assert
        Assert.That(interceptor.MethodSet_Called, Is.EqualTo(1));
        Assert.That(interceptor.Headersready_Called, Is.EqualTo(1));
        Assert.That(interceptor.DataSent_Called, Is.EqualTo(1));
      }
    }


    [Test]
    public void InterceptorMethodsAreCalledOnce_AsyncEvent()
    {
      // Arrange
      var interceptor = new RegisterInterceptor();
      Session.RequestInterceptors.Add(interceptor);

      var request = Session.Bind(VerifiedMethodDossierTemplate, new { method = "POST", id = 8 }).AsJson();

      // Act
      TestAsyncEvent(wh =>
      {
        request.AsyncEvent()
          .OnComplete(() => wh.Set())
          .Post(new { x = 0 }, response => { });
      });

      Assert.That(interceptor.MethodSet_Called, Is.EqualTo(1));
      Assert.That(interceptor.Headersready_Called, Is.EqualTo(1));
      Assert.That(interceptor.DataSent_Called, Is.EqualTo(1));
    }


    [Test]
    public void InterceptorMethodsAreCalledOnceWitNoBody_AsyncEvent()
    {
      // Arrange
      var interceptor = new RegisterInterceptor();
      Session.RequestInterceptors.Add(interceptor);

      var request = Session.Bind(VerifiedMethodDossierTemplate, new { method = "POST", id = 8 }).AsJson();

      // Act
      TestAsyncEvent(wh =>
      {
        request.AsyncEvent()
          .OnComplete(() => wh.Set())
          .Post(response => { });
      });

      Assert.That(interceptor.MethodSet_Called, Is.EqualTo(1));
      Assert.That(interceptor.Headersready_Called, Is.EqualTo(1));
      Assert.That(interceptor.DataSent_Called, Is.EqualTo(1));
    }


    [Test]
    public void InterceptorMethodsAreCalledOnceWithGet_AsyncEvent()
    {
      // Arrange
      var interceptor = new RegisterInterceptor();
      Session.RequestInterceptors.Add(interceptor);

      var request = Session.Bind(VerifiedMethodDossierTemplate, new { method = "GET", id = 8 });

      // Act
      TestAsyncEvent(wh =>
      {
        request.AsyncEvent()
          .OnComplete(() => wh.Set())
          .Get(response => { });
      });

      Assert.That(interceptor.MethodSet_Called, Is.EqualTo(1));
      Assert.That(interceptor.Headersready_Called, Is.EqualTo(1));
      Assert.That(interceptor.DataSent_Called, Is.EqualTo(1));
    }


    [Test]
    public void CanOverrideMethod()
    {
      // Arrange
      Session.RequestInterceptors.Add(new MethodOverrideInterceptor());
      Request r = Session.Bind(HeaderListUrl);
      JsonPatchDocument patch = new JsonPatchDocument();

      // Act
      using (var response = r.Patch<HeaderList>(patch))
      {
        // Assert
        Assert.IsTrue(response.Body.Contains("Method: PATCH"));
      }
    }
  }


  class RegisterInterceptor : IRequestInterceptor2
  {
    public int DataSent_Called = 0;
    public int Headersready_Called = 0;
    public int MethodSet_Called = 0;

    public void DataSent(RequestContext context)
    {
      DataSent_Called++;
    }

    public void HeadersReady(RequestContext context)
    {
      Headersready_Called++;
    }

    public void MethodSet(RequestContext context)
    {
      MethodSet_Called++;
    }
  }


  class MethodOverrideInterceptor : IRequestInterceptor2
  {
    public void DataSent(RequestContext context)
    {
    }

    public void HeadersReady(RequestContext context)
    {
    }

    public void MethodSet(RequestContext context)
    {
      if (context.Request.Method == "PATCH")
      {
        context.Request.Headers["X-HTTP-Method-Override"] = context.Request.Method;
        context.Request.Method = "POST";
      }
    }
  }
}
