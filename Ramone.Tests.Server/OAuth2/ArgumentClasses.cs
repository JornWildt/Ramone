namespace Ramone.Tests.Server.OAuth2
{
  public class OAuth2AccessTokenRequest
  {
    public string grant_type { get; set; }
    public string username { get; set; }
    public string password { get; set; }
  }


  public class OAuth2AccessTokenResponse
  {
    public string access_token { get; set; }
    public string token_type { get; set; }
    public string additional_param { get; set; }
  }


  public class OAuth2Error
  {
    public string error { get; set; }
  }
}