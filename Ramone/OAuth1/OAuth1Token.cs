namespace Ramone.OAuth1
{
  public class OAuth1Token
  {
    public string oauth_token { get; set; }
    public string oauth_token_secret { get; set; }
    public bool oauth_callback_confirmed { get; set; }
  }
}
