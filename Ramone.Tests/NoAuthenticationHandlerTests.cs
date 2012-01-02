using NUnit.Framework;


namespace Ramone.Tests
{
  [TestFixture]
  public class NoAuthenticationHandlerTests : TestHelper
  {
    [Test]
    public void WhenNoAuthorizationCodeIsSendItThrowsRamoneNotAuthorizedException()
    {
      AssertThrows<RamoneNotAuthorizedException>(() => Session.Request(BasicAuthUrl).Get<string>());
    }
  }
}
