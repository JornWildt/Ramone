using System;
using NUnit.Framework;
using Ramone.OAuth2;
using Ramone.Tests.Common.OAuth2;


namespace Ramone.Tests.OAuth2
{
  [TestFixture]
  public class OAuth2AuthorizationCodeGrantTests : TestHelper
  {
    [Test]
    public void WhenRequestingAuthorizationCodeItSavesState()
    {
      // Act
      Uri authUrl =
        Session.OAuth2_Configure(GetSettings())
               .OAuth2_GetAuthorizationRequestUrl();

      OAuth2SessionState state = Session.OAuth2_GetState();

      // Assert
      Assert.IsNotNull(state);
      Assert.IsNotNull(state.AuthorizationState);
      Assert.Greater(state.AuthorizationState.Length, 8);
    }


    [Test]
    public void ItCanReEstablishSessionStateFromAuthorizationRequest()
    {
      // Arrange
      Request protectedResourceRequest = Session.Bind(OAuth2TestConstants.ProtectedResourcePath);

      Session.OAuth2_Configure(GetSettings())
             .OAuth2_GetAuthorizationRequestUrl();

      OAuth2SessionState state = Session.OAuth2_GetState();

      // Act
      ISession newSession = TestService.NewSession();
      newSession.OAuth2_RestoreState(state);

      // Assert ... we don't have a real endpoint for this, so simulate the redirect URL
      string failUrl = "http://localhost?code=xyz&state=123";
      string successUrl = "http://localhost?code=xyz&state=" + state.AuthorizationState;

      string code1 = newSession.OAuth2_GetAuthorizationCodeFromRedirectUrl(failUrl);
      string code2 = newSession.OAuth2_GetAuthorizationCodeFromRedirectUrl(successUrl);

      Assert.IsNull(code1);
      Assert.That(code2, Is.EqualTo("xyz"));
    }
  }
}
