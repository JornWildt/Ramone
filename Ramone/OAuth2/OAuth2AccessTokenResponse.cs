namespace Ramone.OAuth2
{
  public class OAuth2AccessTokenResponse
  {
    public string access_token { get; set; }
    public string token_type { get; set; }
    public int expires_in { get; set; }
    public string refresh_token { get; set; }
  }
}
