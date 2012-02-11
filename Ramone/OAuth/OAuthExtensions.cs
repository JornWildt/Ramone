using CuttingEdge.Conditions;
using Ramone.OAuth.Parameters;
using System;


namespace Ramone.OAuth
{
  public static class OAuthExtensions
  {
    public static void OAuth1Configure(this IRamoneSession session, string consumer_key, string consumer_secret, string callback = null, string access_token = null, string access_token_secret = null)
    {
      Condition.Requires(consumer_key, "consumer_key").IsNotNull();
      Condition.Requires(consumer_key, "consumer_secret").IsNotNull();

      session.RequestInterceptors.Add("OAuth", new OAuthRequestInterceptor(consumer_key, consumer_secret, callback, access_token, access_token_secret));
    }


    public static void OAuth1Token(this IRamoneSession session, TokenResponse token)
    {
      OAuthRequestInterceptor interceptor = session.RequestInterceptors.Find("OAuth") as OAuthRequestInterceptor;
      if (interceptor == null)
        throw new InvalidOperationException("Could not locate OAuth request interceptor. Did you call Session.OAuth1Configure()?");

      interceptor.SetAccessToken(token);
    }
  }
}
