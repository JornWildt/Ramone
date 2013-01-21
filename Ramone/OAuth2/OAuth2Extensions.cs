using CuttingEdge.Conditions;
using System;
using System.Collections.Specialized;
using System.Web;
using System.Diagnostics;


namespace Ramone.OAuth2
{
  public static class OAuth2Extensions
  {
    private const string OAuth2SettingsSessionKey = "OAuth2Settings";


    /// <summary>
    /// Configure OAuth2 and store configuration in session for later use. 
    /// Must always be called before using any of the other OAuth2 methods.
    /// </summary>
    /// <param name="session"></param>
    /// <param name="settings"></param>
    /// <returns>The session passed in as argument.</returns>
    public static ISession OAuth2_Configure(this ISession session, OAuth2Settings settings)
    {
      Condition.Requires(settings, "settings").IsNotNull();

      session.Items[OAuth2SettingsSessionKey] = settings;

      return session;
    }


    /// <summary>
    /// Get URL for user authorization via browser (user agent). This will initiate the "Authorization Code Grant" flow.
    /// </summary>
    /// <remarks>See http://tools.ietf.org/html/rfc6749#section-4.1.1</remarks>
    /// <param name="session"></param>
    /// <param name="scope"></param>
    /// <returns>Authorization request URL.</returns>
    public static Uri OAuth2_GetAuthorizationRequestUrl(this ISession session, string scope = null)
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

      return settings.AuthorizationEndpoint.AddQueryParameters(codeRequestArgs);
    }


    /// <summary>
    /// Extract authorization code from authorization response encoded in a redirect URL from the authorization endpoint.
    /// </summary>
    /// <remarks>After completion of the authorization process the browser will be redirected to a URL specified
    /// by the client (and configured using Ramone's OAuth2Settings). This URL will contain the acquired 
    /// authorization code. Call OAuth2_GetAuthorizationCodeFromRedirectUrl to extract the code.</remarks>
    /// <param name="session"></param>
    /// <param name="redirectUrl"></param>
    /// <returns>Authorization code</returns>
    public static string OAuth2_GetAuthorizationCodeFromRedirectUrl(this ISession session, string redirectUrl)
    {
      Condition.Requires(redirectUrl, "redirectUrl").IsNotNull();

      NameValueCollection parameters = HttpUtility.ParseQueryString(redirectUrl);

      return parameters["code"];
    }


    /// <summary>
    /// Request an access token from authorization code acquired in earlier requests.
    /// </summary>
    /// <remarks>See http://tools.ietf.org/html/rfc6749#section-4.1.3</remarks>
    /// <param name="session"></param>
    /// <param name="authorizationCode"></param>
    /// <param name="useAccessToken">Request automatic use of the returned access token in following requests.</param>
    /// <returns></returns>
    public static OAuth2AccessTokenResponse OAuth2_GetAccessTokenFromAuthorizationCode(
      this ISession session, 
      string authorizationCode,
      bool useAccessToken = true)
    {
      OAuth2Settings settings = GetSettings(session);

      NameValueCollection tokenRequestArgs = new NameValueCollection();
      tokenRequestArgs["grant_type"] = "authorization_code";
      tokenRequestArgs["code"] = authorizationCode;
      tokenRequestArgs["redirect_uri"] = settings.RedirectUri.ToString();
      tokenRequestArgs["client_id"] = settings.ClientID;

      if (!settings.UseBasicAuthenticationForClient)
        tokenRequestArgs["client_secret"] = settings.ClientSecret;

      return GetAndStoreAccessToken(session, tokenRequestArgs, useAccessToken);
    }


    /// <summary>
    /// Request an access token using the flow "Resource Owner Password Credentials Grant".
    /// </summary>
    /// <remarks>See http://tools.ietf.org/html/rfc6749#section-4.3</remarks>
    /// <param name="session"></param>
    /// <param name="ownerUserName"></param>
    /// <param name="ownerPassword"></param>
    /// <param name="useAccessToken">Request automatic use of the returned access token in following requests.</param>
    /// <returns></returns>
    public static OAuth2AccessTokenResponse OAuth2_GetAccessTokenFromResourceUsingOwnerUsernamePassword(
      this ISession session,
      string ownerUserName, 
      string ownerPassword,
      bool useAccessToken = true)
    {
      OAuth2Settings settings = GetSettings(session);

      NameValueCollection tokenRequestArgs = new NameValueCollection();
      tokenRequestArgs["grant_type"] = "password";
      tokenRequestArgs["username"] = ownerUserName;
      tokenRequestArgs["password"] = ownerPassword;

      if (!settings.UseBasicAuthenticationForClient)
      {
        tokenRequestArgs["client_id"] = settings.ClientID;
        tokenRequestArgs["client_secret"] = settings.ClientSecret;
      }

      return GetAndStoreAccessToken(session, tokenRequestArgs, useAccessToken);
    }


    /// <summary>
    /// Does this session have an active access token associated with it?
    /// </summary>
    /// <param name="session"></param>
    /// <returns></returns>
    public static bool OAuth2_HasActiveAccessToken(this ISession session)
    {
      return session.RequestInterceptors.Find("Bearer") != null;
    }


    /// <summary>
    /// Get a copy of the OAuth2 settings (use OAuth2_Configure to change them)
    /// </summary>
    /// <param name="session"></param>
    /// <returns></returns>
    public static OAuth2Settings OAuth2_GetSettings(this ISession session)
    {
      object settings;
      session.Items.TryGetValue(OAuth2SettingsSessionKey, out settings);
      return settings as OAuth2Settings == null ? null : new OAuth2Settings((OAuth2Settings)settings);
    }


    private static OAuth2AccessTokenResponse GetAndStoreAccessToken(ISession session, object args, bool useAccessToken)
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
        if (useAccessToken)
        {
          if (string.Equals(accessToken.token_type, "bearer", StringComparison.InvariantCultureIgnoreCase))
          {
            session.RequestInterceptors.Add("Bearer", new BearerTokenRequestInterceptor(accessToken.access_token));
          }
          else
            throw new InvalidOperationException(string.Format("Unknown access token type '{0}' (expected 'bearer')", accessToken.token_type));
        }
        return accessToken;
      }
    }


    private static OAuth2Settings GetSettings(ISession session)
    {
      object settings;
      if (!session.Items.TryGetValue(OAuth2SettingsSessionKey, out settings))
        throw new InvalidOperationException("No OAuth2 settings has been registered with the session");

      if (settings is OAuth2Settings)
        return (OAuth2Settings)settings;
      
      throw new InvalidOperationException(string.Format("Unknown type '{0}' has been registered for OAuth2 settings with the session", settings.GetType()));
    }
  }
}
