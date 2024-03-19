using System;
using NUnit.Framework;
using Ramone.OAuth2;
using Ramone.Tests.Common.OAuth2;


namespace Ramone.Tests.OAuth2
{
  [TestFixture]
  public class OAuth2RefreshTokenTests : TestHelper
  {
    [Test]
    public void CanUseRefreshTokenToGetNewAccessToken()
    {
      // Arrange

      // Act
      OAuth2AccessTokenResponse token = Session.OAuth2_Configure(GetSettings())
        .OAuth2_RefreshAccessToken("myrefreshtoken");

      // Assert
      Assert.That(token, Is.Not.Null);
    }
  }
}
