using NUnit.Framework;
using Ramone.Tests.Common.OAuth2;


namespace Ramone.Tests.OAuth2
{
  [TestFixture]
  public class OAuth2ResourceOwnerPasswordCredentialsGrantTests : TestHelper
  {
    [Test]
    public void CanGetAccessToken()
    {
      Request request = Session.Bind(OAuth2Constants.TokenEndpointPath)
                               .BasicAuthentication(OAuth2Constants.ClientID, OAuth2Constants.ClientPassword)
                               .AsFormUrlEncoded()
                               .AcceptJson();

      var tokenRequest = new 
      { 
        grant_type = "password", 
        username = OAuth2Constants.Username, 
        password = OAuth2Constants.UserPassword
      };

      using (var response = request.Post<dynamic>(tokenRequest))
      {
        Assert.IsNotNull(response.Body);
        Assert.IsNotNullOrEmpty(response.Body.access_token);
      }
    }
  }
}
