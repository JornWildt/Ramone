using System;


namespace Ramone.OAuth2
{
  public class OAuth2Settings
  {
    public Uri AuthorizationEndpoint { get; set; }
    public Uri TokenEndpoint { get; set; }
    public Uri RedirectUri { get; set; }
    public string ClientID { get; set; }
    public string ClientSecret { get; set; }

    public bool UseBasicAuthenticationForClient { get; set; }
  }
}
