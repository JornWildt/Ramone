using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using Ramone.Tests.Common;
using Ramone.Tests.Common.CMS;

namespace Ramone.Tests
{
  [TestFixture]
  public class ResponseInterceptorTests : TestHelper
  {
    public List<HttpStatusCode> StatusCodes;


    protected override void SetUp()
    {
      base.SetUp();
      StatusCodes = new List<HttpStatusCode>();
    }


    [Test]
    public void CanReadStatusCode()
    {
      // Arrange
      Session.ResponseInterceptors.Add(new TestInterceptor(this));
      Request dossierReq = Session.Bind(DossierTemplate, new { id = 8 });

      // Act
      using (var dossier = dossierReq.Get<Dossier>())
      {
        // Assert
        Assert.AreEqual(1, StatusCodes.Count);
        Assert.AreEqual(HttpStatusCode.OK, StatusCodes[0]);
      }
    }


    [Test]
    public void ResponseInterceptorsAreCalledOnRedirects()
    {
      // Arrange
      Session.ResponseInterceptors.Add(new TestInterceptor(this));
      Request req = Session.Bind(RedirectTemplate, new { code = 301, count = 1, v = 0 });

      // Act
      using (var resp = req.Get())
      {
        // Assert
        Assert.AreEqual(5, StatusCodes.Count);
        Assert.AreEqual(HttpStatusCode.MovedPermanently, StatusCodes[0]);
        Assert.AreEqual(HttpStatusCode.MovedPermanently, StatusCodes[1]);
        Assert.AreEqual(HttpStatusCode.MovedPermanently, StatusCodes[2]);
        Assert.AreEqual(HttpStatusCode.MovedPermanently, StatusCodes[3]);
        Assert.AreEqual(HttpStatusCode.OK, StatusCodes[4]);
      }
    }


    [Test]
    public async Task ResponseInterceptorsAreCalled_Async()
    {
      // Arrange
      Session.ResponseInterceptors.Add(new TestInterceptor(this));
      Request dossierReq = Session.Bind(DossierTemplate, new { id = 8 });

      // Act
      using (var dossier = await dossierReq.Async().Get<Dossier>())
      {
        // Assert
        Assert.AreEqual(1, StatusCodes.Count);
        Assert.AreEqual(HttpStatusCode.OK, StatusCodes[0]);
      }
    }


    [Test]
    public void ResponseInterceptorsAreCalled_AsyncEvent()
    {
      // Arrange
      Session.ResponseInterceptors.Add(new TestInterceptor(this));
      Request dossierReq = Session.Bind(DossierTemplate, new { id = 8 });

      // Act
      TestAsyncEvent(wh =>
      {
        dossierReq.AsyncEvent().Get<Dossier>(dossier =>
        {
          // Assert
          Assert.AreEqual(1, StatusCodes.Count);
          Assert.AreEqual(HttpStatusCode.OK, StatusCodes[0]);
          wh.Set();
        });
      });
    }
  }


  class TestInterceptor : IResponseInterceptor
  {
    ResponseInterceptorTests Test;

    public TestInterceptor(ResponseInterceptorTests test)
    {
      Test = test;
    }


    public void ResponseReady(ResponseContext response)
    {
      Test.StatusCodes.Add(response.Response.StatusCode);
    }
  }
}
