namespace Ramone.OAuth
{
  public static class OAuthExtensions
  {
    public static void OAuth1Start(this IRamoneSession session, string consumer_key, string consumer_secret, string callback = null, string access_token = null, string access_token_secret = null)
    {
      session.RequestInterceptors.Add("OAuth", new OAuthRequestInterceptor(consumer_key, consumer_secret, callback, access_token, access_token_secret));
    }
  }
}
