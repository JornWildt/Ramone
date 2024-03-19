using System;
using System.Net;
using System.Text;
using NUnit.Framework;
using Ramone.Implementation;
using Ramone.AuthorizationInterceptors;


namespace Ramone.Tests
{
  [TestFixture]
  public class AuthenticationTests : TestHelper
  {
    [Test]
    public void WhenAuthorizationCodeIsSendItWorks()
    {
      Session.RequestInterceptors.Add("WhenAuthorizationCodeIsSendItWorks", new BasicAuthorizationInterceptor("John", "magic"));
      using (var respone = Session.Request(BasicAuthUrl).Get<string>())
        Assert.That(respone.Body, Is.Not.Null);
    }


    [Test]
    public void WhenAuthorizationCodeIsSendItWorks_AsyncEvent()
    {
      Session.RequestInterceptors.Add("WhenAuthorizationCodeIsSendItWorks", new BasicAuthorizationInterceptor("John", "magic"));

      TestAsyncEvent(wh =>
      {
        // Act
        Session.Request(BasicAuthUrl).AsyncEvent().Get<string>(response =>
        {
          Assert.That(response.Body, Is.Not.Null);
          wh.Set();
        });
      });
    }


    [Test]
    public void CanAddAuthorizerToSession()
    {
      Session.BasicAuthentication("John", "magic");
      using (var respone = Session.Request(BasicAuthUrl).Get<string>())
        Assert.That(respone.Body, Is.Not.Null);
    }


    [Test]
    public void CanAddAuthorizerToSessionMoreThanOnce()
    {
      Session.BasicAuthentication("John", "magic");
      Session.BasicAuthentication("Steinbeck", "more magic");
      AssertThrows<WebException>(() => Session.Request(BasicAuthUrl).Get<string>(),
        ex => ((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.Unauthorized);
    }


    [Test]
    public void CanAddAuthorizerToSession_AsyncEvent()
    {
      Session.BasicAuthentication("John", "magic");
      TestAsyncEvent(wh =>
      {
        // Act
        Session.Request(BasicAuthUrl).AsyncEvent().Get<string>(response =>
        {
          Assert.That(response.Body, Is.Not.Null);
          wh.Set();
        });
      });
    }


    [Test]
    public void CanAddAuthorizerToService()
    {
      // Arrange
      IService service = RamoneConfiguration.NewService(BaseUrl);

      // Act
      service.BasicAuthentication("John", "magic");
      ISession session = service.NewSession();
      using (Response response = session.Request(BasicAuthUrl).Get())
      {
        // Assert
        Assert.That(response, Is.Not.Null);
      }
    }


    [Test]
    public void CanAddAuthorizerWithInternationalLettersToService()
    {
      // Arrange
      IService service = RamoneConfiguration.NewService(BaseUrl);

      // Act
      service.BasicAuthentication("Jürgen Wølst", "hmpf");
      ISession session = service.NewSession();
      using (Response response = session.Request(BasicAuthUrl).Get())
      {
        // Assert
        Assert.That(response, Is.Not.Null);
      }
    }


    [Test]
    public void CanAddAuthorizerToRequest()
    {
      // Act
      using (Response response = Session.Request(BasicAuthUrl).BasicAuthentication("John", "magic").Get())
      {
        // Assert
        Assert.That(response, Is.Not.Null);
      }
    }


    [Test]
    public void CanAddAuthorizerToRequest_AsyncEvent()
    {
      TestAsyncEvent(wh =>
      {
        // Act
        Session.Request(BasicAuthUrl).BasicAuthentication("John", "magic").AsyncEvent().Get<string>(response =>
        {
          Assert.That(response.Body, Is.Not.Null);
          wh.Set();
        });
      });
    }


    [Test]
    public void CanAddAuthorizerWithInternationalLettersToRequest()
    {
      // Act
      using (Response response = Session.Request(BasicAuthUrl).BasicAuthentication("Jürgen Wølst", "hmpf").Get())
      {
        // Assert
        Assert.That(response, Is.Not.Null);
      }
    }


    [Test]
    public void WhenAskedForAuthorizationAndAnsweredItGetsAccess()
    {
      // Throws first time
      AssertThrowsWebException(() => Session.Request(BasicAuthUrl).Get<string>(), HttpStatusCode.Unauthorized);

      // Then we assign a authorization handler - and now we gain access
      Session.AuthorizationDispatcher.Add("basic", new BasicAuthorizationHandler());

      using (var response = Session.Request(BasicAuthUrl).Get<string>())
        Assert.That(response.Body, Is.Not.Null);
    }


    [Test]
    public void WhenAskedForAuthorizationAndAnsweredItGetsAccess_AsyncEvent()
    {
      // Throws first time
      bool? failedAsExpected = null;
      TestAsyncEvent(wh =>
      {
        // Act
        Session.Request(BasicAuthUrl).AsyncEvent().OnError(error =>
        {
          failedAsExpected = (HttpStatusCode.Unauthorized == error.Response.StatusCode);
          wh.Set();
        }).Get<string>(response => {});
      });

      Assert.That(failedAsExpected, Is.EqualTo(true));

      // Throws first time
      AssertThrowsWebException(() => Session.Request(BasicAuthUrl).Get<string>(), HttpStatusCode.Unauthorized);

      // Then we assign a authorization handler - and now we gain access
      Session.AuthorizationDispatcher.Add("basic", new BasicAuthorizationHandler());

      bool succeededAsExpected = false;

      TestAsyncEvent(wh =>
      {
        // Act
        Session.Request(BasicAuthUrl).AsyncEvent().Get<string>(response => 
        {
          succeededAsExpected = true;
          wh.Set();
        });
      });

      Assert.That(succeededAsExpected, Is.True);
    }


    public class BasicAuthorizationHandler : IAuthorizationHandler
    {
      #region IAuthorizationHandler Members

      public bool HandleAuthorizationRequest(AuthorizationContext context)
      {
        context.Session.RequestInterceptors.Add("WhenAuthorizationCodeIsSendItWorks", new BasicAuthorizationInterceptor("John", "magic"));
        return true;
      }

      #endregion
    }
  }
}
