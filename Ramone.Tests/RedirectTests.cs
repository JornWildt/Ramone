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


    static object[] InvalidRedirectCases = 
    {
      new object[] { 301, "POST" },
      new object[] { 302, "POST" },
      new object[] { 307, "POST" },
      new object[] { 301, "PUT" },
      new object[] { 302, "PUT" },
      new object[] { 307, "PUT" }
    };

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
