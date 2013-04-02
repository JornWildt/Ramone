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

    static object[] ValidRedirectCases = 
    {
      new object[] { 301, "GET" },
      new object[] { 302, "GET" },
      new object[] { 303, "GET" },
      new object[] { 307, "GET" },
      new object[] { 301, "HEAD" },
      new object[] { 302, "HEAD" },
      new object[] { 303, "HEAD" },
      new object[] { 307, "HEAD" },
      new object[] { 303, "POST" },
      new object[] { 303, "PUT" }
    };


    static object[] InvalidRedirectCases = 
    {
      new object[] { 301, "POST" },
      new object[] { 302, "POST" },
      new object[] { 307, "POST" },
      new object[] { 301, "PUT" },
      new object[] { 302, "PUT" },
      new object[] { 307, "PUT" }
    };


    [Test, TestCaseSource("ValidRedirectCases")]
    public void ItFollowsRedirectOnValidMethodsAndStatuses(int responseCode, string method)
    {
      // Arrange
      Request req = Session.Bind(RedirectTemplate, new { code = responseCode, count = 1 });

      // Act
      using (Response<RedirectArgs> resp = req.Execute<RedirectArgs>(method))
      {
        // Assert
        Assert.AreEqual(4, resp.RedirectCount);
      }
    }


    [Test, TestCaseSource("ValidRedirectCases")]
    public void ItFollowsRedirectOnValidMethodsAndStatuses_Async(int responseCode, string method)
    {
      // Arrange
      Request req = Session.Bind(RedirectTemplate, new { code = responseCode, count = 1 });

      // Act
      TestAsync(wh =>
        {
          req.Async().Execute<RedirectArgs>(method, response =>
            {
              // Assert
              Assert.AreEqual(4, response.RedirectCount);
              wh.Set();
            });
        });
    }


    [Test, TestCaseSource("InvalidRedirectCases")]
    public void ItDoesNotFollowRedirectOnInvalidMethodsAndStatuses(int responseCode, string method)
    {
      // Arrange
      Request req = Session.Bind(RedirectTemplate, new { code = responseCode, count = 1 });

      // Act
      using (Response<RedirectArgs> resp = req.Execute<RedirectArgs>(method))
      {
        // Assert
        Assert.AreEqual(0, resp.RedirectCount);
      }
    }


    [Test, TestCaseSource("InvalidRedirectCases")]
    public void ItDoesNotFollowRedirectOnInvalidMethodsAndStatuses_Async(int responseCode, string method)
    {
      // Arrange
      Request req = Session.Bind(RedirectTemplate, new { code = responseCode, count = 1 });

      // Act
      TestAsync(wh =>
        {
          req.Async().Execute<RedirectArgs>(method, response =>
            {
              // Assert
              Assert.AreEqual(0, response.RedirectCount);
              wh.Set();
            });
        });
    }


    static object[] AFewValidRedirectCases = 
    {
      new object[] { 301, "GET" },
      new object[] { 307, "HEAD" },
      new object[] { 301, "GET" },
      new object[] { 303, "PUT" },
      new object[] { 303, "POST" }
    };


    [Test, TestCaseSource("AFewValidRedirectCases")]
    public void RedirectCountCanBeSpecified(int responseCode, string method)
    {
      // Arrange
      Session.SetAllowedRedirects(responseCode, 2);
      Request req = Session.Bind(RedirectTemplate, new { code = responseCode, count = 1 });

      // Act
      using (Response<RedirectArgs> resp = req.Execute<RedirectArgs>(method))
      {
        // Assert
        Assert.AreEqual(2, resp.RedirectCount);
      }
    }


    [Test, TestCaseSource("AFewValidRedirectCases")]
    public void RedirectCountCanBeSpecified_Async(int responseCode, string method)
    {
      // Arrange
      Session.SetAllowedRedirects(responseCode, 2);
      Request req = Session.Bind(RedirectTemplate, new { code = responseCode, count = 1 });

      // Act
      TestAsync(wh =>
      {
        req.Async().Execute<RedirectArgs>(method, response =>
        {
          // Assert
          Assert.AreEqual(2, response.RedirectCount);
          wh.Set();
        });
      });
    }


    [Test, TestCaseSource("AFewValidRedirectCases")]
    public void WithRedirectCountSetToZeroItDoesNotFollowRedirects(int responseCode, string method)
    {
      // Arrange
      Request req = Session.Bind(RedirectTemplate, new { code = responseCode, count = 1 });
      Session.SetAllowedRedirects(responseCode, 0);

      // Act
      using (Response<RedirectArgs> resp = req.Execute<RedirectArgs>(method))
      {
        // Assert
        Assert.AreEqual(0, resp.RedirectCount);
        Assert.AreEqual(responseCode, (int)resp.WebResponse.StatusCode);
      }
    }


    [Test, TestCaseSource("AFewValidRedirectCases")]
    public void WithRedirectCountSetToZeroItDoesNotFollowRedirects_Async(int responseCode, string method)
    {
      // Arrange
      Request req = Session.Bind(RedirectTemplate, new { code = responseCode, count = 1 });
      Session.SetAllowedRedirects(responseCode, 0);

      // Act
      TestAsync(wh =>
      {
        req.Async().Execute<RedirectArgs>(method, response =>
        {
          // Assert
          Assert.AreEqual(0, response.RedirectCount);
          Assert.AreEqual(responseCode, (int)response.WebResponse.StatusCode);
          wh.Set();
        });
      });
    }


    [Test, TestCaseSource("AFewValidRedirectCases")]
    public void WhenFollowingRedirectsItAppliesRequestInterceptors(int responseCode, string method)
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


    [Test, TestCaseSource("AFewValidRedirectCases")]
    public void WhenFollowingRedirectsItAppliesRequestInterceptors_Async(int responseCode, string method)
    {
      // Arrange
      Request req = Session.Bind(RedirectTemplate, new { code = responseCode, count = 1 });
      Session.SetAllowedRedirects(responseCode, 5);
      Session.RequestInterceptors.Add(new RequestInterceptor());
      InterceptorCount = 0;

      // Act
      TestAsync(wh =>
      {
        req.Async().Execute<RedirectArgs>(method, response =>
        {
          // Assert
          Assert.AreEqual(5, InterceptorCount);
          wh.Set();
        });
      });
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
