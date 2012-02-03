using NUnit.Framework;


namespace Ramone.Tests
{
  [TestFixture]
  public class NoAuthenticationHandlerTests : TestHelper
  {
    [Test]
    public void WhenNoAuthorizationCodeIsSendItThrowsRamoneNotAuthorizedException()
    {
      AssertThrows<NotAuthorizedException>(() => Session.Request(BasicAuthUrl).Get<string>());
    }
  }
}
