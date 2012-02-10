namespace Ramone.OAuth.Parameters
{
  public class TokenResponse
  {
    public string oauth_token { get; set; }
    public string oauth_token_secret { get; set; }
    public bool oauth_callback_confirmed { get; set; }
  }
}
