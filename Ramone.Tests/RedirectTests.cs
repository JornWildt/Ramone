using NUnit.Framework;
using Ramone.Tests.Common;


namespace Ramone.Tests
{
  [TestFixture]
  public class RedirectTests : TestHelper
  {
    protected override void TestFixtureSetUp()
    {
      base.TestFixtureSetUp();
      TestService.CodecManager.AddFormUrlEncoded<RedirectArgs>();
    }


    [Test]
    public void ByDefaultItFollowsRedirectsFor303WithGET(
      [Values(303)] int responseCode,
      [Values("GET", "POST")] string method)
    {
      // Arrange
      Request req = Session.Bind(RedirectTemplate, new { code = responseCode, count = 1 });

      // Act
      using (Response<RedirectArgs> resp = req.Execute<RedirectArgs>(method))
      {
        // Assert
        Assert.AreEqual(5, resp.Body.Count, "Must have been redirected 4 times (server max.).");
        Assert.AreEqual(4, resp.RedirectCount);
        Assert.AreEqual("GET", resp.Body.Method);
      }
    }


    [Test]
    public void ByDefaultItDoesNotFollowRedirectsForNon303(
      [Values(301, 302, 307)] int responseCode,
      [Values("GET", "POST")] string method)
    {
      // Arrange
      Request req = Session.Bind(RedirectTemplate, new { code = responseCode, count = 1 });
      
      // Act
      using (Response<RedirectArgs> resp = req.Execute<RedirectArgs>(method))
      {
        // Assert
        Assert.AreEqual(1, resp.Body.Count);
      }
    }


    [Test]
    public void RedirectCountCanBeSpecified(
      [Values(301, 307)] int responseCode1,
      [Values(301, 307)] int responseCode2,
      [Values("GET", "POST")] string method)
    {
      if (responseCode1 != responseCode2)
      {
        // Arrange
        Request req1 = Session.Bind(RedirectTemplate, new { code = responseCode1, count = 1 });
        Request req2 = Session.Bind(RedirectTemplate, new { code = responseCode2, count = 1 });

        Session.SetAllowedRedirects(responseCode1, 2);

        // Act
        using (Response<RedirectArgs> resp1 = req1.Execute<RedirectArgs>(method))
        using (Response<RedirectArgs> resp2 = req2.Execute<RedirectArgs>(method))
        {
          // Assert
          Assert.AreEqual(3, resp1.Body.Count, "Must have been redirected 2 times as specified.");
          Assert.AreEqual(2, resp1.RedirectCount);
          Assert.AreEqual(1, resp2.Body.Count, "Must not redirect other codes.");
          Assert.AreEqual(0, resp2.RedirectCount);
        }
      }
    }


    [Test]
    public void WithRedirectCountSetToZeroItDoesNotFollowRedirects(
      [Values(301, 307)] int responseCode,
      [Values("GET", "POST")] string method)
    {
      // Arrange
      Request req = Session.Bind(RedirectTemplate, new { code = responseCode, count = 1 });
      Session.SetAllowedRedirects(responseCode, 0);

      // Act
      using (Response<RedirectArgs> resp = req.Execute<RedirectArgs>(method))
      {
        // Assert
        Assert.AreEqual(1, resp.Body.Count);
        Assert.AreEqual(responseCode, (int)resp.WebResponse.StatusCode);
      }
    }


    [Test]
    public void WhenFollowingRedirectsItAppliesRequestInterceptors(
      [Values(301, 307)] int responseCode,
      [Values("GET", "POST")] string method)
    {
      // Arrange
      Request req = Session.Bind(RedirectTemplate, new { code = responseCode, count = 1 });
      Session.SetAllowedRedirects(responseCode, 5);
      Session.RequestInterceptors.Add(new RequestInterceptor());
      InterceptorCount = 0;

      // Act
      using (Response<RedirectArgs> resp = req.Execute<RedirectArgs>(method))
      {
        // Assert
        Assert.AreEqual(5, InterceptorCount);
      }
    }


    static int InterceptorCount;


    class RequestInterceptor : IRequestInterceptor
    {
      public void HeadersReady(RequestContext context)
      {
        ++InterceptorCount;
      }

      public void DataSent(RequestContext context)
      {
      }
    }

  }
}
