using System;
using System.Net;
using System.Text;
using NUnit.Framework;
using Ramone.Implementation;


namespace Ramone.Tests
{
  [TestFixture]
  public class AuthenticationHandlerTests : TestHelper
  {
    protected override void SetUp()
    {
      base.SetUp();
      CountingAuthorizationHandler.Count = 0;
      Session.AuthorizationDispatcher.Add("basic", new CountingAuthorizationHandler());
    }


    [Test]
    public void WhenNoAuthorizationCodeIsSendItAsksForAuthorization()
    {
      AssertThrowsWebException(() => Session.Request(BasicAuthUrl).Get<string>(), HttpStatusCode.Unauthorized);
      // Will get called twice since it does not try to fix the access problem
      Assert.That(CountingAuthorizationHandler.Count, Is.EqualTo(2));
    }


    [Test]
    public void WhenNoAuthorizationCodeIsSendItAsksForAuthorization_AsyncEvent()
    {
      Response errorResponse = null;

      TestAsyncEvent(wh =>
      {
        Session.Request(BasicAuthUrl).AsyncEvent()
          .OnError(error => errorResponse = error.Response)
          .OnComplete(() => wh.Set())
          .Get<string>(r => 
          {
          });
      });

      Assert.IsNotNull(errorResponse);
      Assert.That(errorResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));

      // Will get called twice since it does not try to fix the access problem
      Assert.That(CountingAuthorizationHandler.Count, Is.EqualTo(2));
    }


    public class CountingAuthorizationHandler : IAuthorizationHandler
    {
      public static int Count = 0;

      #region IAuthorizationHandler Members

      public bool HandleAuthorizationRequest(AuthorizationContext context)
      {
        ++Count;
        return true;
      }

      #endregion

    }
  }
}
