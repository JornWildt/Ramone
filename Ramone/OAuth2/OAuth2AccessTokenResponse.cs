namespace Ramone.OAuth2
{
  /// <summary>
  /// A class representing the response from an access token request.
  /// </summary>
  /// <remarks>See http://tools.ietf.org/html/rfc6749#section-4.1.4</remarks>
  public class OAuth2AccessTokenResponse
  {
    public string access_token { get; set; }
    public string token_type { get; set; }
    public int expires_in { get; set; }
    public string refresh_token { get; set; }
  }
}
