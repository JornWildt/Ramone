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
      Session.AuthorizationDispatcher.Add("basic", new CountingAuthorizationHandler());
    }


    [Test]
    public void WhenNoAuthorizationCodeIsSendItAsksForAuthorization()
    {
      AssertThrows<RamoneNotAuthorizedException>(() => Session.Request(BasicAuthUrl).Get<string>());
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
