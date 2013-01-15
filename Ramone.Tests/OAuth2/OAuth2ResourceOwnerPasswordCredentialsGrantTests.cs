using NUnit.Framework;
using Ramone.Tests.Common.OAuth2;
using Ramone.OAuth2;
using System.Net;


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
               .OAuth2_GetAccessTokenUsing_ResourceOwnerPasswordCredentialsGrant(OAuth2TestConstants.Username, OAuth2TestConstants.UserPassword);

      Assert.IsNotNull(token);
      Assert.IsNotNullOrEmpty(token.access_token);
    }


    [Test]
    public void WhenItHasAccessTokenItCanAccessProtectedResource()
    {
      // Arrange
      Request protectedResourceRequest = Session.Bind(OAuth2TestConstants.ProtectedResourcePath);

      AssertThrows<NotAuthorizedException>(() => protectedResourceRequest.Get());

      // Act
      Session.OAuth2_Configure(GetSettings())
              .OAuth2_GetAccessTokenUsing_ResourceOwnerPasswordCredentialsGrant(OAuth2TestConstants.Username, OAuth2TestConstants.UserPassword);

      using (var response = protectedResourceRequest.AcceptJson().Get<ProtectedResource>())
      {
        ProtectedResource r = response.Body;

        // Assert
        Assert.IsNotNull(r);
        Assert.AreEqual("Got it", r.Title);
      }
    }


    private OAuth2Settings GetSettings()
    {
      return new OAuth2Settings
      {
        TokenEndpoint = new System.Uri(Session.BaseUri, OAuth2TestConstants.TokenEndpointPath),
        ClientID = OAuth2TestConstants.ClientID,
        ClientSecret = OAuth2TestConstants.ClientPassword
      };
    }
  }
}
