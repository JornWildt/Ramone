using NUnit.Framework;
using Ramone.Tests.Common.OAuth2;
using Ramone.OAuth2;


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
    public void CanGetAccessTokenUsingOAuth2()
    {
      OAuth2Settings settings = new OAuth2Settings
      {
        TokenEndpoint = new System.Uri(Session.BaseUri, OAuth2TestConstants.TokenEndpointPath),
        ClientID = OAuth2TestConstants.ClientID,
        ClientSecret = OAuth2TestConstants.ClientPassword
      };

      //Session.OAuth2Configure(settings)

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
  }
}
