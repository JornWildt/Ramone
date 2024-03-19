using System.Collections.Generic;
using System.Net;
using NUnit.Framework;
using Ramone.OAuth2;
using Ramone.Tests.Common.OAuth2;


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
        Assert.That(response.Body, Is.Not.Null);
        Assert.That(response.Body.access_token, Is.Not.Null.And.Not.Empty);
      }
    }


    [Test]
    public void CanGetAccessTokenWithAdditionalParametersUsingOAuth2Extensions()
    {
      OAuth2AccessTokenResponse token = 
        Session.OAuth2_Configure(GetSettings())
               .OAuth2_GetAccessTokenUsingOwnerUsernamePassword(
                 OAuth2TestConstants.Username, 
                 OAuth2TestConstants.UserPassword,
                 extraRequestArgs: new Dictionary<string, string> { ["additional"] = "Even more special" });

      Assert.That(token, Is.Not.Null);
      Assert.That(token.access_token, Is.Not.Null.And.Not.Empty);
      Assert.That(token.expires_in, Is.EqualTo(199));
      Assert.That((string)token.AllParameters["additional_param"], Is.EqualTo("Even more special"));
    }


    [Test]
    public void CanGetAccessTokenUsingNullPassword()
    {
      OAuth2AccessTokenResponse token =
        Session.OAuth2_Configure(GetSettings())
               .OAuth2_GetAccessTokenUsingOwnerUsernamePassword(OAuth2TestConstants.UsernameWithEmptyPassword, null);

      Assert.That(token, Is.Not.Null);
      Assert.That(token.access_token, Is.Not.Null.And.Not.Empty);
    }


    [Test]
    public void WhenItHasAccessTokenItCanAccessProtectedResource()
    {
      // Arrange
      Request protectedResourceRequest = Session.Bind(OAuth2TestConstants.ProtectedResourcePath);

      AssertThrowsWebException(() => protectedResourceRequest.Get(), HttpStatusCode.Unauthorized);

      // Act
      Session.OAuth2_Configure(GetSettings())
              .OAuth2_GetAccessTokenUsingOwnerUsernamePassword(OAuth2TestConstants.Username, OAuth2TestConstants.UserPassword);

      using (var response = protectedResourceRequest.AcceptJson().Get<ProtectedResource>())
      {
        ProtectedResource r = response.Body;

        // Assert
        Assert.That(r, Is.Not.Null);
        Assert.That(r.Title, Is.EqualTo("Got it"));
      }
    }


    [Test]
    public void CanAvoidAutomaticUseOfAccessToken()
    {
      // Arrange
      Request protectedResourceRequest = Session.Bind(OAuth2TestConstants.ProtectedResourcePath);

      // Act
      Session.OAuth2_Configure(GetSettings())
              .OAuth2_GetAccessTokenUsingOwnerUsernamePassword(OAuth2TestConstants.Username, OAuth2TestConstants.UserPassword,
                                                               useAccessToken: false);

      AssertThrowsWebException(() => protectedResourceRequest.Get(), HttpStatusCode.Unauthorized);
    }


    [Test]
    public void CanRestoreSessionStateWithAccessToken()
    {
      // Arrange
      Session.OAuth2_Configure(GetSettings())
              .OAuth2_GetAccessTokenUsingOwnerUsernamePassword(OAuth2TestConstants.Username, OAuth2TestConstants.UserPassword);

      OAuth2SessionState state = Session.OAuth2_GetState();

      // Act
      ISession newSession = TestService.NewSession();

      Request protectedResourceRequest = newSession.OAuth2_RestoreState(state).Bind(OAuth2TestConstants.ProtectedResourcePath);

      using (var response = protectedResourceRequest.AcceptJson().Get<ProtectedResource>())
      {
        ProtectedResource r = response.Body;

        // Assert
        Assert.That(r, Is.Not.Null);
        Assert.That(r.Title, Is.EqualTo("Got it"));
      }
    }
  }
}
