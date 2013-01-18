using NUnit.Framework;
using Ramone.Tests.Common.OAuth2;
using Ramone.OAuth2;
using System.Net;
using System;


namespace Ramone.Tests.OAuth2
{
  [TestFixture]
  public class OAuth2ResourceOwnerPasswordCredentialsGrantTests : TestHelper
  {
    [Test]
    public void CanGetAccessTokenUsingStandardPrimitives()
    {
      Request request = Session.Bind(OAuth2TestConstants.TokenEndpointPath)
                               .BasicAuthentication(OAuth2TestConstants.ClientID, OAuth2TestConstants.ClientPassword)
                               .AsFormUrlEncoded()
                               .AcceptJson();

      var tokenRequest = new 
      { 
        grant_type = "password", 
        username = OAuth2TestConstants.Username, 
        password = OAuth2TestConstants.UserPassword
      };

      using (var response = request.Post<dynamic>(tokenRequest))
      {
        Assert.IsNotNull(response.Body);
        Assert.IsNotNullOrEmpty(response.Body.access_token);
      }
    }


    [Test]
    public void CanGetAccessTokenUsingOAuth2Extensions()
    {
      OAuth2AccessTokenResponse token = 
        Session.OAuth2_Configure(GetSettings())
               .OAuth2_GetAccessTokenFromResourceOwnerUsernamePassword(OAuth2TestConstants.Username, OAuth2TestConstants.UserPassword);

      Assert.IsNotNull(token);
      Assert.IsNotNullOrEmpty(token.access_token);
    }


    [Test]
    public void WhenItHasAccessTokenItCanAccessProtectedResource()
    {
      // Arrange
      Request protectedResourceRequest = Session.Bind(OAuth2TestConstants.ProtectedResourcePath);

      AssertThrowsWebException(() => protectedResourceRequest.Get(), HttpStatusCode.Unauthorized);

      // Act
      Session.OAuth2_Configure(GetSettings())
              .OAuth2_GetAccessTokenFromResourceOwnerUsernamePassword(OAuth2TestConstants.Username, OAuth2TestConstants.UserPassword);

      using (var response = protectedResourceRequest.AcceptJson().Get<ProtectedResource>())
      {
        ProtectedResource r = response.Body;

        // Assert
        Assert.IsNotNull(r);
        Assert.AreEqual("Got it", r.Title);
      }
    }


    [Test]
    public void CanAvoidAutomaticUseOfAccessToken()
    {
      // Arrange
      Request protectedResourceRequest = Session.Bind(OAuth2TestConstants.ProtectedResourcePath);

      // Act
      Session.OAuth2_Configure(GetSettings())
              .OAuth2_GetAccessTokenFromResourceOwnerUsernamePassword(OAuth2TestConstants.Username, OAuth2TestConstants.UserPassword,
                                                                      useAccessToken: false);

      AssertThrowsWebException(() => protectedResourceRequest.Get(), HttpStatusCode.Unauthorized);
    }


    [Test]
    public void CanQuestionOAuth2State()
    {
      // Arrange
      Request protectedResourceRequest = Session.Bind(OAuth2TestConstants.ProtectedResourcePath);

      // Act
      bool isActive1 = Session.OAuth2_HasActiveAccessToken();

      Session.OAuth2_Configure(GetSettings())
              .OAuth2_GetAccessTokenFromResourceOwnerUsernamePassword(OAuth2TestConstants.Username, OAuth2TestConstants.UserPassword);

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
              .OAuth2_GetAccessTokenFromResourceOwnerUsernamePassword(OAuth2TestConstants.Username, OAuth2TestConstants.UserPassword);

      OAuth2Settings settings2 = Session.OAuth2_GetSettings();

      // Assert
      Assert.IsNull(settings1);
      Assert.IsNotNull(settings2);
    }


    [Test]
    public void WhenDoingOAuth2BeforeConfiguringItThrowsInvalidOperation()
    {
      AssertThrows<InvalidOperationException>(
        () => Session.OAuth2_GetAccessTokenFromResourceOwnerUsernamePassword(OAuth2TestConstants.Username, OAuth2TestConstants.UserPassword));
    }


    private OAuth2Settings GetSettings()
    {
      return new OAuth2Settings
      {
        TokenEndpoint = new System.Uri(Session.BaseUri, OAuth2TestConstants.TokenEndpointPath),
        ClientID = OAuth2TestConstants.ClientID,
        ClientSecret = OAuth2TestConstants.ClientPassword,
        UseBasicAuthenticationForClient = true
      };
    }
  }
}
