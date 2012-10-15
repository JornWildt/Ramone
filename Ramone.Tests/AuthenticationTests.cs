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
    public void WhenAskedForAuthorizationAndAnsweredItGetsAccess()
    {
      // Throws first time
      AssertThrows<NotAuthorizedException>(() => Session.Request(BasicAuthUrl).Get<string>());

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
