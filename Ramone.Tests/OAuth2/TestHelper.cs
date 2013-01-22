using System;
using Ramone.OAuth2;
using Ramone.Tests.Common.OAuth2;


namespace Ramone.Tests.OAuth2
{
  public class TestHelper : Ramone.Tests.TestHelper
  {
    protected OAuth2Settings GetSettings()
    {
      return new OAuth2Settings
      {
        AuthorizationEndpoint = new System.Uri(Session.BaseUri, OAuth2TestConstants.TokenEndpointPath),
        TokenEndpoint = new System.Uri(Session.BaseUri, OAuth2TestConstants.TokenEndpointPath),
        ClientID = OAuth2TestConstants.ClientID,
        ClientSecret = OAuth2TestConstants.ClientPassword,
        UseBasicAuthenticationForClient = true,
        RedirectUri = new Uri("http://dr.dk")
      };
    }
  }
}
