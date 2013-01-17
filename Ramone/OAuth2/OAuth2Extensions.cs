using CuttingEdge.Conditions;
using System;
using System.Collections.Specialized;
using System.Web;
using System.Diagnostics;


namespace Ramone.OAuth2
{
  public static class OAuth2Extensions
  {
    private const string OAuth2SettingsKey = "OAuth2Settings";


    public static ISession OAuth2_Configure(this ISession session, OAuth2Settings settings)
    {
      Condition.Requires(settings, "settings").IsNotNull();

      session.Items[OAuth2SettingsKey] = settings;

      return session;
    }


    public static OAuth2AuthorizationRedirect OAuth2_AuthorizeWithRedirect(this ISession session, string scope = null)
    {
      OAuth2Settings settings = GetSettings(session);

      var codeRequestArgs = new
      {
        response_type = "code",
        client_id = settings.ClientID,
        redirect_uri = settings.RedirectUri.ToString(),
        scope = scope,
        state = "123456" // FIXME : see http://tools.ietf.org/html/rfc6749#section-10.12
      };

      using (var response = session.Bind(settings.AuthorizationEndpoint.AddQueryParameters(codeRequestArgs)).Get())
      {
        return new OAuth2AuthorizationRedirect { Location = response.Location };
      }
    }

    // FIXME: how to, automaticall, decode redirect response (when possible)?
    //NameValueCollection responseQuery = HttpUtility.ParseQueryString(response.Location.Query);
    //return new OAuth2AuthorizationCodeResponse
    //{
    //  code = responseQuery["code"],
    //  state = responseQuery["state"]
    //};

    // FIXME: handle additional access token parameters

    public static OAuth2AccessTokenResponse OAuth2_GetAccessTokenFromAuthorizationCode(this ISession session, string authorizationCode)
    {
      OAuth2Settings settings = GetSettings(session);

      NameValueCollection tokenRequestArgs = new NameValueCollection();
      tokenRequestArgs["grant_type"] = "authorization_code";
      tokenRequestArgs["code"] = authorizationCode;
      tokenRequestArgs["redirect_uri"] = settings.RedirectUri.ToString();
      tokenRequestArgs["client_id"] = settings.ClientID;

      if (!settings.UseBasicAuthenticationForClient)
        tokenRequestArgs["client_secret"] = settings.ClientSecret;

      return GetAccessToken(session, tokenRequestArgs);
    }


    public static OAuth2AccessTokenResponse OAuth2_GetAccessTokenFromResourceOwnerUsernamePassword(
      this ISession session,
      string ownerUserName, 
      string ownerPassword)
    {
      OAuth2Settings settings = GetSettings(session);

      NameValueCollection tokenRequestArgs = new NameValueCollection();
      tokenRequestArgs["grant_type"] = "password";
      tokenRequestArgs["username"] = ownerUserName;
      tokenRequestArgs["password"] = ownerPassword;

      if (!settings.UseBasicAuthenticationForClient)
        tokenRequestArgs["client_secret"] = settings.ClientSecret;

      return GetAccessToken(session, tokenRequestArgs);
    }


    private static OAuth2AccessTokenResponse GetAccessToken(ISession session, object args)
    {
      OAuth2Settings settings = GetSettings(session);

      Request request = session.Bind(settings.TokenEndpoint)
                               .AsFormUrlEncoded()
                               .AcceptJson();

      if (settings.UseBasicAuthenticationForClient)
        request = request.BasicAuthentication(settings.ClientID, settings.ClientSecret);

      using (var response = request.Post<OAuth2AccessTokenResponse>(args))
      {
        OAuth2AccessTokenResponse accessToken = response.Body;
        if (string.Equals(accessToken.token_type, "bearer", StringComparison.InvariantCultureIgnoreCase))
        {
          session.RequestInterceptors.Add("Bearer", new BearerTokenRequestInterceptor(accessToken.access_token));
          return accessToken;
        }
        else
          throw new InvalidOperationException(string.Format("Unknown access token type '{0}' (expected 'bearer')", accessToken.token_type));
      }
    }


    private static OAuth2Settings GetSettings(ISession session)
    {
      object settings = session.Items[OAuth2SettingsKey];
      if (settings == null)
        throw new InvalidOperationException("No OAuth2 settings has been registered with the session");

      if (settings is OAuth2Settings)
        return (OAuth2Settings)settings;
      
      throw new InvalidOperationException(string.Format("Unknown type '{0}' has been registered for OAuth2 settings with the session", settings.GetType()));
    }
  }
}
