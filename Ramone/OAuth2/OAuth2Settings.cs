using System;


namespace Ramone.OAuth2
{
  /// <summary>
  /// Various settings for getting OAuth2 up and running.
  /// </summary>
  public class OAuth2Settings
  {
    /// <summary>
    /// Authorization endpoint URL as specified in RFC 6749.
    /// </summary>
    /// <remarks>See http://tools.ietf.org/html/rfc6749#section-3.1</remarks>
    public Uri AuthorizationEndpoint { get; set; }

    /// <summary>
    /// Token endpoint URL as specified in RFC 6749.
    /// </summary>
    /// <remarks>See http://tools.ietf.org/html/rfc6749#section-3.2</remarks>
    public Uri TokenEndpoint { get; set; }

    /// <summary>
    /// Client redirection endpoint URL as specified in RFC 6749.
    /// </summary>
    /// <remarks>See http://tools.ietf.org/html/rfc6749#section-3.1.2</remarks>
    public Uri RedirectUri { get; set; }

    /// <summary>
    /// Client identifier as specified in RFC 6749.
    /// </summary>
    /// <remarks>See http://tools.ietf.org/html/rfc6749#section-2.2</remarks>
    public string ClientID { get; set; }

    /// <summary>
    /// Client secret (password) as specified in RFC 6749.
    /// </summary>
    /// <remarks>See http://tools.ietf.org/html/rfc6749#section-2.3.1</remarks>
    public string ClientSecret { get; set; }

    /// <summary>
    /// Specify whether or not to use HTTP Basic authentication for client credentials.
    /// </summary>
    /// <remarks>See http://tools.ietf.org/html/rfc6749#section-2.3.1</remarks>
    public bool UseBasicAuthenticationForClient { get; set; }
  }
}
