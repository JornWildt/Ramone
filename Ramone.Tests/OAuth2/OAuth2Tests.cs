using System;
using NUnit.Framework;
using Ramone.OAuth2;
using Ramone.Tests.Common.OAuth2;


namespace Ramone.Tests.OAuth2
{
  [TestFixture]
  public class OAuth2Tests : TestHelper
  {
    [Test]
    public void CanQuestionOAuth2State()
    {
      // Arrange
      Request protectedResourceRequest = Session.Bind(OAuth2TestConstants.ProtectedResourcePath);

      // Act
      bool isActive1 = Session.OAuth2_HasActiveAccessToken();

      Session.OAuth2_Configure(GetSettings())
              .OAuth2_GetAccessTokenFromResourceUsingOwnerUsernamePassword(OAuth2TestConstants.Username, OAuth2TestConstants.UserPassword);

      bool isActive2 = Session.OAuth2_HasActiveAccessToken();

      // Assert
      Assert.IsFalse(isActive1);
      Assert.IsTrue(isActive2);
    }


    [Test]
    public void CanGetCurrentOAuth2Settings()
    {
      // Arrange
      Request protectedResourceRequest = Session.Bind(OAuth2TestConstants.ProtectedResourcePath);

      // Act
      OAuth2Settings settings1 = Session.OAuth2_GetSettings();

      Session.OAuth2_Configure(GetSettings())
              .OAuth2_GetAccessTokenFromResourceUsingOwnerUsernamePassword(OAuth2TestConstants.Username, OAuth2TestConstants.UserPassword);

      OAuth2Settings settings2 = Session.OAuth2_GetSettings();

      // Assert
      Assert.IsNull(settings1);
      Assert.IsNotNull(settings2);
    }


    [Test]
    public void WhenGettingSettingsItReturnsACopy()
    {
      // Arrange
      Session.OAuth2_Configure(GetSettings());

      // Act
      OAuth2Settings settings1 = Session.OAuth2_GetSettings();
      settings1.ClientID = "NEWCID";
      OAuth2Settings settings2 = Session.OAuth2_GetSettings();

      // Assert
      Assert.AreEqual(OAuth2TestConstants.ClientID, settings2.ClientID);
      Assert.AreEqual(settings2.ClientSecret, settings2.ClientSecret);
      Assert.AreEqual(settings2.AuthorizationEndpoint, settings2.AuthorizationEndpoint);
      Assert.AreEqual(settings2.RedirectUri, settings2.RedirectUri);
      Assert.AreEqual(settings2.TokenEndpoint, settings2.TokenEndpoint);
      Assert.AreEqual(settings2.UseBasicAuthenticationForClient, settings2.UseBasicAuthenticationForClient);
    }


    [Test]
    public void WhenDoingOAuth2BeforeConfiguringItThrowsInvalidOperation()
    {
      AssertThrows<InvalidOperationException>(
        () => Session.OAuth2_GetAccessTokenFromResourceUsingOwnerUsernamePassword(OAuth2TestConstants.Username, OAuth2TestConstants.UserPassword));
    }
  }
}
