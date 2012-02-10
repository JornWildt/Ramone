namespace Ramone.OAuth
{
  public static class OAuthExtensions
  {
    public static void OAuth1Start(this IRamoneSession session, string consumer_key, string consumer_secret)
    {
      session.RequestInterceptors.Add("OAuth", new OAuthRequestInterceptor(consumer_key, consumer_secret));
    }
  }
}
