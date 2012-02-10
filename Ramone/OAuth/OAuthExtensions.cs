using CuttingEdge.Conditions;
namespace Ramone.OAuth
{
  public static class OAuthExtensions
  {
    public static void OAuth1Start(this IRamoneSession session, string consumer_key, string consumer_secret, string callback = null, string access_token = null, string access_token_secret = null)
    {
      Condition.Requires(consumer_key, "consumer_key").IsNotNull();
      Condition.Requires(consumer_key, "consumer_secret").IsNotNull();

      session.RequestInterceptors.Add("OAuth", new OAuthRequestInterceptor(consumer_key, consumer_secret, callback, access_token, access_token_secret));
    }


    public static void OAuth1Token(this IRamoneSession session, string token, string token_secret)
    {
      //session.RequestInterceptors.Add("OAuth", new OAuthRequestInterceptor(consumer_key, consumer_secret, callback, access_token, access_token_secret));
    }
  }
}
