using System;
using CuttingEdge.Conditions;
using Ramone.MediaTypes.FormUrlEncoded;


namespace Ramone.OAuth1
{
  public static class OAuth1Extensions
  {
    public static void OAuth1Configure(this ISession session, OAuth1Settings settings)
    {
      // Ignore returned media types from servers when fetching request/access-tokens
      // (This is so silly: Twitter returning text/html when it is application/x-www-form-urlencoded.
      // See https://dev.twitter.com/discussions/5662)
      session.Service.CodecManager.AddFormUrlEncoded<OAuth1Token>(MediaType.Wildcard);

      Condition.Requires(settings.ConsumerKey, "settings.ConsumerKey").IsNotNull();
      Condition.Requires(settings.ConsumerSecrect, "settings.ConsumerSecrect").IsNotNull();
      Condition.Requires(settings.RequestTokenUrl, "settings.RequestTokenUrl").IsNotNull();
      Condition.Requires(settings.AuthorizeUrl, "settings.AuthorizeUrl").IsNotNull();
      Condition.Requires(settings.AccessTokenUrl, "settings.AccessTokenUrl").IsNotNull();

      session.RequestInterceptors.Add("OAuth", new OAuth1RequestInterceptor(settings));
    }


    public static void OAuth1Logger(this ISession session, IOAuth1Logger logger)
    {
      OAuth1RequestInterceptor interceptor = GetExistingInterceptor(session);
      interceptor.Logger = logger;
    }


    public static OAuth1Token OAuth1GetRequestToken(this ISession session, bool rememberToken = true)
    {
      OAuth1Settings settings = GetExistingSettings(session);

      OAuth1Token token = session.Request(settings.RequestTokenUrl)
                                 .Accept<OAuth1Token>()
                                 .Post()
                                 .Body;

      if (rememberToken)
        OAuth1SetAccessToken(session, token);

      return token;
    }


    public static OAuth1Token OAuth1GetAccessTokenFromRequestToken(this ISession session, string verifier, bool rememberToken = true)
    {
      OAuth1Settings settings = GetExistingSettings(session);

      OAuth1Token token = session.Bind(settings.AccessTokenUrl, new { oauth_verifier = verifier })
                                  .Accept<OAuth1Token>()
                                  .Post()
                                  .Body;

      if (rememberToken)
        OAuth1SetAccessToken(session, token, true);

      return token;
    }


    public static void OAuth1SetAccessToken(this ISession session, OAuth1Token token, bool isAuthorized = false)
    {
      OAuth1RequestInterceptor interceptor = GetExistingInterceptor(session);
      interceptor.SetAccessToken(token);
      interceptor.IsAuthorized = isAuthorized;
    }


    public static bool OAuth1IsAuthorized(this ISession session)
    {
      OAuth1RequestInterceptor interceptor = GetExistingInterceptor(session);
      return interceptor.IsAuthorized;
    }


    private static OAuth1Settings GetExistingSettings(ISession session)
    {
      OAuth1RequestInterceptor interceptor = GetExistingInterceptor(session);
      return interceptor.Settings;
    }


    private static OAuth1RequestInterceptor GetExistingInterceptor(ISession session)
    {
      OAuth1RequestInterceptor interceptor = session.RequestInterceptors.Find("OAuth") as OAuth1RequestInterceptor;
      if (interceptor == null)
        throw new InvalidOperationException("Could not locate OAuth request interceptor. Did you call Session.OAuth1Configure()?");
      return interceptor;
    }
  }
}
