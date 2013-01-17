using NUnit.Framework;
using System.Net;


namespace Ramone.Tests
{
  [TestFixture]
  public class NoAuthenticationHandlerTests : TestHelper
  {
    [Test]
    public void WhenNoAuthorizationCodeIsSendItThrowsWebException()
    {
      AssertThrowsWebException(() => Session.Request(BasicAuthUrl).Get<string>(), HttpStatusCode.Unauthorized);
    }
  }
}
