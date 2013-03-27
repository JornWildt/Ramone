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
      Assert.AreEqual(2, CountingAuthorizationHandler.Count);
    }


    [Test]
    public void WhenNoAuthorizationCodeIsSendItAsksForAuthorization_Async()
    {
      Response errorResponse = null;

      TestAsync(wh =>
      {
        Session.Request(BasicAuthUrl).Async()
          .OnComplete(() => wh.Set())
          .OnError(resp => errorResponse = resp)
          .Get<string>(r => {});
      });

      Assert.IsNotNull(errorResponse);
      Assert.AreEqual(HttpStatusCode.Unauthorized, errorResponse.StatusCode);

      // Will get called twice since it does not try to fix the access problem
      Assert.AreEqual(2, CountingAuthorizationHandler.Count);
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
