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
      string result = Session.Request(BasicAuthUrl).Get<string>().Body;
      Assert.IsNotNull(result);
    }


    [Test]
    public void WhenAskedForAuthorizationAndAnsweredItGetsAccess()
    {
      // Throws first time
      AssertThrows<RamoneNotAuthorizedException>(() => Session.Request(BasicAuthUrl).Get<string>());

      // Then we assign a authorization handler - and now we gain access
      Session.AuthorizationDispatcher.Add("basic", new BasicAuthorizationHandler());

      string result = Session.Request(BasicAuthUrl).Get<string>().Body;
      Assert.IsNotNull(result);
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
