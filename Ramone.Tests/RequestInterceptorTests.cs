using NUnit.Framework;
using Ramone.MediaTypes.JsonPatch;
using Ramone.Tests.Common;


namespace Ramone.Tests
{
  [TestFixture]
  public class RequestInterceptorTests : TestHelper
  {
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
