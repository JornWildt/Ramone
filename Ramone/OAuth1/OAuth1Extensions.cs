using System;
using CuttingEdge.Conditions;


namespace Ramone.OAuth1
{
  public static class OAuth1Extensions
  {
    public static void OAuth1Configure(this IRamoneSession session, OAuth1Settings settings)
    {
      Condition.Requires(settings.ConsumerKey, "settings.ConsumerKey").IsNotNull();
      Condition.Requires(settings.ConsumerSecrect, "settings.ConsumerSecrect").IsNotNull();
      Condition.Requires(settings.RequestTokenUrl, "settings.RequestTokenUrl").IsNotNull();
      Condition.Requires(settings.AuthorizeUrl, "settings.AuthorizeUrl").IsNotNull();
      Condition.Requires(settings.AccessTokenUrl, "settings.AccessTokenUrl").IsNotNull();

      session.RequestInterceptors.Add("OAuth", new OAuth1RequestInterceptor(settings));
    }


    public static OAuth1Token OAuth1GetRequestToken(this IRamoneSession session, bool rememberToken = true)
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


    public static OAuth1Token OAuth1GetAccessTokenFromRequestToken(this IRamoneSession session, string verifier, bool rememberToken = true)
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


    public static void OAuth1SetAccessToken(this IRamoneSession session, OAuth1Token token, bool isAuthorized = false)
    {
      OAuth1RequestInterceptor interceptor = GetExistingInterceptor(session);
      interceptor.SetAccessToken(token);
      interceptor.IsAuthorized = isAuthorized;
    }


    public static bool OAuth1IsAuthorized(this IRamoneSession session)
    {
      OAuth1RequestInterceptor interceptor = GetExistingInterceptor(session);
      return interceptor.IsAuthorized;
    }


    private static OAuth1Settings GetExistingSettings(IRamoneSession session)
    {
      OAuth1RequestInterceptor interceptor = GetExistingInterceptor(session);
      return interceptor.Settings;
    }


    private static OAuth1RequestInterceptor GetExistingInterceptor(IRamoneSession session)
    {
      OAuth1RequestInterceptor interceptor = session.RequestInterceptors.Find("OAuth") as OAuth1RequestInterceptor;
      if (interceptor == null)
        throw new InvalidOperationException("Could not locate OAuth request interceptor. Did you call Session.OAuth1Configure()?");
      return interceptor;
    }
  }
}
