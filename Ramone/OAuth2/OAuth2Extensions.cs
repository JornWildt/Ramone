using CuttingEdge.Conditions;
using System;
using System.Collections.Specialized;


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


    public static OAuth2AuthorizationCodeResponse OAuth2_Authorize(this ISession session, string scope = null)
    {
      OAuth2Settings settings = GetSettings(session);

      NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(settings.AuthorizationEndpoint.Query);

      queryString["response_type"] = "code";
      queryString["client_id"] = settings.ClientID;
      queryString["redirect_uri"] = settings.RedirectUri.ToString();
      queryString["scope"] = scope;
      queryString["state"] = "1234";

      string q = queryString.ToString();

      Request request = session.Bind(settings.AuthorizationEndpoint)
                               .AsFormUrlEncoded();



      //var tokenRequest = new
      //{
      //  response_type = "code",
      //  client_id = settings.ClientID,
      //  redirect_uri = settings.RedirectUri,
      //  scope = scope,
      //  state = "123456"
      //};

      //using (var response = request.Post<OAuth2AccessTokenResponse>(tokenRequest))
      //{
      //  OAuth2AccessTokenResponse accessToken = response.Body;
      //  if (string.Equals(accessToken.token_type, "bearer", StringComparison.InvariantCultureIgnoreCase))
      //  {
      //    session.RequestInterceptors.Add("Bearer", new BearerTokenRequestInterceptor(accessToken.access_token));
      //    return accessToken;
      //  }
      //  else
      //    throw new RamoneException(string.Format("Unknown access token type '{0}' (expected 'bearer')", accessToken.token_type));
      //}

      return null;
    }


    public static OAuth2AccessTokenResponse OAuth2_GetAccessTokenUsing_ResourceOwnerPasswordCredentialsGrant(
      this ISession session,
      string ownerUserName, 
      string ownerPassword)
    {
      OAuth2Settings settings = GetSettings(session);

      Request request = session.Bind(settings.TokenEndpoint)
                               .BasicAuthentication(settings.ClientID, settings.ClientSecret)
                               .AsFormUrlEncoded()
                               .AcceptJson();

      var tokenRequest = new
      {
        grant_type = "password",
        username = ownerUserName,
        password = ownerPassword
      };

      using (var response = request.Post<OAuth2AccessTokenResponse>(tokenRequest))
      {
        OAuth2AccessTokenResponse accessToken = response.Body;
        if (string.Equals(accessToken.token_type, "bearer", StringComparison.InvariantCultureIgnoreCase))
        {
          session.RequestInterceptors.Add("Bearer", new BearerTokenRequestInterceptor(accessToken.access_token));
          return accessToken;
        }
        else
          throw new RamoneException(string.Format("Unknown access token type '{0}' (expected 'bearer')", accessToken.token_type));
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
