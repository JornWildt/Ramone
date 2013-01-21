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
        Assert.IsNotNull(respone.Body);
    }


    [Test]
    public void CanAddAuthorizerToSession()
    {
      Session.BasicAuthentication("John", "magic");
      using (var respone = Session.Request(BasicAuthUrl).Get<string>())
        Assert.IsNotNull(respone.Body);
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
        Assert.IsNotNull(response);
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
        Assert.IsNotNull(response);
      }
    }


    [Test]
    public void CanAddAuthorizerToRequest()
    {
      // Act
      using (Response response = Session.Request(BasicAuthUrl).BasicAuthentication("John", "magic").Get())
      {
        // Assert
        Assert.IsNotNull(response);
      }
    }


    [Test]
    public void CanAddAuthorizerWithInternationalLettersToRequest()
    {
      // Act
      using (Response response = Session.Request(BasicAuthUrl).BasicAuthentication("Jürgen Wølst", "hmpf").Get())
      {
        // Assert
        Assert.IsNotNull(response);
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
        Assert.IsNotNull(response.Body);
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
