using System;
using System.Linq;
using System.Collections.Specialized;
using System.Web;
using CuttingEdge.Conditions;
using Ramone.Utility;
using Microsoft.CSharp.RuntimeBinder;
using System.Collections;
using System.Security.Cryptography;
using Ramone.Utility.JsonWebToken;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Reflection;


namespace Ramone.OAuth2
{
  public enum AccessTokenType { Bearer }

  public enum ClientAuthenticationFlowType { Rfc7523Section21, Rfc7521Section62 }

  public static class OAuth2Extensions
  {
    private const string OAuth2SettingsSessionKey = "OAuth2Settings";
    private const string OAuth2StateSessionKey = "OAuth2State";

    // ECDsaCertificateExtensions was introduced in .NET 4.6.1
    private static Lazy<Func<X509Certificate2, ECDsa>> GetECDsaPublicKey = new Lazy<Func<X509Certificate2, ECDsa>>(() =>
    {
      Type t = Type.GetType("System.Security.Cryptography.X509Certificates.ECDsaCertificateExtensions, System.Core, Version = 4.0.0.0, Culture = neutral, PublicKeyToken = b77a5c561934e089");
      if (t == null)
        return null;
      MethodInfo mi = t.GetMethod("GetECDsaPrivateKey", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(X509Certificate2) }, null);
      if (mi == null)
        return null;
      return (Func<X509Certificate2, ECDsa>)mi.CreateDelegate(typeof(Func<X509Certificate2, ECDsa>));
    }, System.Threading.LazyThreadSafetyMode.PublicationOnly);

    /// <summary>
    /// Configure OAuth2 and store configuration in session for later use. 
    /// Must always be called before using any of the other OAuth2 methods.
    /// </summary>
    /// <param name="session">Ramone session.</param>
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
    /// <param name="session">Ramone session.</param>
    /// <param name="scope">Space separated list of strings identifying the required scopes (as defined by the authorization server).</param>
    /// <returns>Authorization request URL.</returns>
    public static Uri OAuth2_GetAuthorizationRequestUrl(this ISession session, string scope = null)
    {
      OAuth2Settings settings = GetSettings(session);

      string authorizationRequestState = RandomStrings.GetRandomStringWithLettersAndDigitsOnly(20);
      OAuth2SessionState state = session.OAuth2_GetOrCreateState();
      state.AuthorizationState = authorizationRequestState;

      var codeRequestArgs = new
      {
        response_type = "code",
        client_id = settings.ClientID,
        redirect_uri = settings.RedirectUri.ToString(),
        scope = scope,
        state = authorizationRequestState
      };

      return settings.AuthorizationEndpoint.AddQueryParameters(codeRequestArgs);
    }


    /// <summary>
    /// Extract authorization code from authorization response encoded in a redirect URL from the authorization endpoint.
    /// </summary>
    /// <remarks>After completion of the authorization process the browser will be redirected to a URL specified
    /// by the client (and configured using Ramone's OAuth2Settings). This URL will contain the acquired 
    /// authorization code. Call OAuth2_GetAuthorizationCodeFromRedirectUrl to extract the code.</remarks>
    /// <param name="session">Ramone session.</param>
    /// <param name="redirectUrl"></param>
    /// <returns>Authorization code</returns>
    public static string OAuth2_GetAuthorizationCodeFromRedirectUrl(this ISession session, string redirectUrl)
    {
      Condition.Requires(redirectUrl, "redirectUrl").IsNotNull();

      OAuth2SessionState sessionState = session.OAuth2_GetState();

      NameValueCollection parameters = HttpUtility.ParseQueryString(new Uri(redirectUrl).Query);

      string state = parameters["state"];
      if (sessionState.AuthorizationState == null || state != sessionState.AuthorizationState)
        return null;

      return parameters["code"];
    }


    /// <summary>
    /// Request an access token from authorization code acquired in earlier requests.
    /// </summary>
    /// <remarks>See http://tools.ietf.org/html/rfc6749#section-4.1.3</remarks>
    /// <param name="session">Ramone session.</param>
    /// <param name="authorizationCode"></param>
    /// <param name="extraRequestArgs">Optionally specify extra arguments in the POST data of the HTTP request.</param>
    /// <param name="useAccessToken">Request automatic use of the returned access token in following requests.</param>
    /// <returns></returns>
    public static OAuth2AccessTokenResponse OAuth2_GetAccessTokenFromAuthorizationCode(
      this ISession session, 
      string authorizationCode,
      IDictionary<string, string> extraRequestArgs = null,
      bool useAccessToken = true)
    {
      OAuth2Settings settings = GetSettings(session);

      NameValueCollection tokenRequestArgs = new NameValueCollection();
      tokenRequestArgs["grant_type"] = "authorization_code";
      tokenRequestArgs["code"] = authorizationCode;
      tokenRequestArgs["redirect_uri"] = settings.RedirectUri.ToString();

      if (extraRequestArgs != null)
      {
        foreach (var kv in extraRequestArgs)
          tokenRequestArgs[kv.Key] = kv.Value;
      }

      if (settings.ClientAuthenticationMethod == OAuth2Settings.DefaultClientAuthenticationMethods.RequestBody)
      {
        tokenRequestArgs["client_id"] = settings.ClientID;
        tokenRequestArgs["client_secret"] = settings.ClientSecret;
      }

      return GetAndStoreAccessToken(session, tokenRequestArgs, useAccessToken);
    }


    /// <summary>
    /// Request an access token using the flow "Resource Owner Password Credentials Grant".
    /// </summary>
    /// <remarks>See http://tools.ietf.org/html/rfc6749#section-4.3</remarks>
    /// <param name="session">Ramone session.</param>
    /// <param name="ownerUserName"></param>
    /// <param name="ownerPassword"></param>
    /// <param name="scope">Space separated list of strings identifying the required scopes (as defined by the authorization server).</param>
    /// <param name="extraRequestArgs">Optionally specify extra arguments in the POST data of the HTTP request.</param>
    /// <param name="useAccessToken">Request automatic use of the returned access token in following requests.</param>
    /// <returns></returns>
    public static OAuth2AccessTokenResponse OAuth2_GetAccessTokenUsingOwnerUsernamePassword(
      this ISession session,
      string ownerUserName, 
      string ownerPassword,
      string scope = null,
      IDictionary<string, string> extraRequestArgs = null,
      bool useAccessToken = true)
    {
      OAuth2Settings settings = GetSettings(session);

      NameValueCollection tokenRequestArgs = new NameValueCollection();
      tokenRequestArgs["grant_type"] = "password";
      tokenRequestArgs["username"] = ownerUserName;
      if (ownerPassword != null)
        tokenRequestArgs["password"] = ownerPassword;
      if (scope != null)
        tokenRequestArgs["scope"] = scope;

      if (extraRequestArgs != null)
      {
        foreach (var kv in extraRequestArgs)
          tokenRequestArgs[kv.Key] = kv.Value;
      }

      if (settings.ClientAuthenticationMethod == OAuth2Settings.DefaultClientAuthenticationMethods.RequestBody)
      {
        tokenRequestArgs["client_id"] = settings.ClientID;
        tokenRequestArgs["client_secret"] = settings.ClientSecret;
      }

      return GetAndStoreAccessToken(session, tokenRequestArgs, useAccessToken);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="session">Ramone session.</param>
    /// <param name="scope">Space separated list of strings identifying the required scopes (as defined by the authorization server).</param>
    /// <param name="extraRequestArgs">Optionally specify extra arguments in the POST data of the HTTP request.</param>
    /// <param name="useAccessToken">Store the returned access token in session and use that in future requests to the resource server.</param>
    /// <returns></returns>
    public static OAuth2AccessTokenResponse OAuth2_GetAccessTokenUsingClientCredentials(
      this ISession session, 
      string scope = null,
      IDictionary<string, string> extraRequestArgs = null,
      bool useAccessToken = true)
    {
      OAuth2Settings settings = GetSettings(session);

      NameValueCollection tokenRequestArgs = new NameValueCollection();
      tokenRequestArgs["grant_type"] = "client_credentials";
      if (scope != null)
        tokenRequestArgs["scope"] = scope;

      if (extraRequestArgs != null)
      {
        foreach (var kv in extraRequestArgs)
          tokenRequestArgs[kv.Key] = kv.Value;
      }

      if (settings.ClientAuthenticationMethod == OAuth2Settings.DefaultClientAuthenticationMethods.RequestBody)
      {
        tokenRequestArgs["client_id"] = settings.ClientID;
        tokenRequestArgs["client_secret"] = settings.ClientSecret;
      }

      return GetAndStoreAccessToken(session, tokenRequestArgs, useAccessToken);
    }


    /// <summary>
    /// Get an access token using the flow "Client Credentials Grant" with SHA256 signed JWT client credentials.
    /// </summary>
    /// <param name="session">Ramone session.</param>
    /// <param name="shaKey">Binary representation of the SHA256 key.</param>
    /// <param name="args">Assertion arguments</param>
    /// <param name="useAccessToken">Store the returned access token in session and use that in future requests to the resource server.</param>
    /// <returns></returns>
    public static OAuth2AccessTokenResponse OAuth2_GetAccessTokenFromJWT_SHA256(
      this ISession session, 
      byte[] shaKey, 
      AssertionArgs args, 
      bool useAccessToken = true)
    {
      return OAuth2_GetAccessTokenFromJWT(session, Jose.JwsAlgorithm.HS256, shaKey, args, useAccessToken);
    }


    /// <summary>
    /// Get an access token using the flow "Client Credentials Grant" with RAS-SHA256 signed JWT client credentials.
    /// </summary>
    /// <param name="session">Ramone session.</param>
    /// <param name="cp">RSA Crypto provider used to do the RSA-SHA256 signing.</param>
    /// <param name="args">Assertion arguments.</param>
    /// <param name="useAccessToken">Store the returned access token in session and use that in future requests to the resource server.</param>
    /// <returns></returns>
    public static OAuth2AccessTokenResponse OAuth2_GetAccessTokenFromJWT_RSASHA256(
      this ISession session, 
      RSACryptoServiceProvider cp, 
      AssertionArgs args, 
      bool useAccessToken = true)
    {
      return OAuth2_GetAccessTokenFromJWT(session, Jose.JwsAlgorithm.RS256, cp, args, useAccessToken);
    }


    /// <summary>
    /// Get an access token using either the flow "JWTs as Authorization Grants" defined in RFC 7523 Section 2.1 or
    /// "Client Acting on Behalf of Itself" defined in RFC 7521 Section 6.2 with JWT client credentials signed with 
    /// an algorithm appropiate for the supplied certificate.
    /// </summary>
    /// <param name="session">Ramone session.</param>
    /// <param name="key">The key used by the signing algorithm.</param>
    /// <param name="args">Assertion arguments.</param>
    /// <param name="flowType">Specify which client authentication flow to use.</param>
    /// <param name="extraHeaders">Optionally specify extra headers in the assertion.</param>
    /// <param name="extraClaims">Optionally specify extra claims in the assertion.</param>
    /// <param name="extraRequestArgs">Optionally specify extra arguments in the POST data of the HTTP request.</param>
    /// <param name="useAccessToken">Store the returned access token in session and use that in future requests to the resource server.</param>
    /// <returns></returns>
    /// <remarks>For RSA the number of signature bits are independent of the keysize, this function will always use RS256 for RSA.</remarks>
    public static OAuth2AccessTokenResponse OAuth2_GetAccessTokenFromJWT_ByCertificate(
      this ISession session, 
      X509Certificate2 cert, 
      AssertionArgs args,
      ClientAuthenticationFlowType flowType,
      IDictionary<string, object> extraHeaders = null, 
      IDictionary<string, object> extraClaims = null, 
      IDictionary<string, string> extraRequestArgs = null,
      bool useAccessToken = true)
    {
      if (cert == null)
        throw new ArgumentNullException(nameof(cert));
      if (!cert.HasPrivateKey)
        throw new InvalidOperationException("The certificate does not contain a private key");
      AsymmetricAlgorithm key = cert.GetRSAPrivateKey();
      if (key == null)
        key = GetECDsaPublicKey.Value?.Invoke(cert);
      if (key == null)
        throw new NotSupportedException(string.Format("Unsupported private key: {0}", cert.PrivateKey));
      return OAuth2_GetAccessTokenFromJWT_ByKey(session, key, args, flowType, extraHeaders, extraClaims, extraRequestArgs, useAccessToken);
    }

    /// <summary>
    /// Get an access token using either the flow "JWTs as Authorization Grants" defined in RFC 7523 Section 2.1 or
    /// "Client Acting on Behalf of Itself" defined in RFC 7521 Section 6.2 with JWT client credentials signed with 
    /// an algorithm appropiate for the supplied key.
    /// </summary>
    /// <param name="session">Ramone session.</param>
    /// <param name="key">The key used by the signing algorithm.</param>
    /// <param name="args">Assertion arguments.</param>
    /// <param name="flowType">Specify which client authentication flow to use.</param>
    /// <param name="extraHeaders">Optionally specify extra headers in the assertion.</param>
    /// <param name="extraClaims">Optionally specify extra claims in the assertion.</param>
    /// <param name="extraRequestArgs">Optionally specify extra arguments in the POST data of the HTTP request.</param>
    /// <param name="useAccessToken">Store the returned access token in session and use that in future requests to the resource server.</param>
    /// <returns></returns>
    /// <remarks>For RSA the number of signature bits are independent of the keysize, this function will always use RS256 for RSA.</remarks>
    public static OAuth2AccessTokenResponse OAuth2_GetAccessTokenFromJWT_ByKey(
      this ISession session, 
      object key, 
      AssertionArgs args,
      ClientAuthenticationFlowType flowType,
      IDictionary<string, object> extraHeaders = null, 
      IDictionary<string, object> extraClaims = null, 
      IDictionary<string, string> extraRequestArgs = null,
      bool useAccessToken = true)
    {
      object testKey = key ?? throw new ArgumentNullException(nameof(key));

      if (testKey is CngKey cngKey)
      {
        if (cngKey.AlgorithmGroup.Equals(CngAlgorithmGroup.ECDiffieHellman) || cngKey.AlgorithmGroup.Equals(CngAlgorithmGroup.ECDsa))
          testKey = new ECDsaCng(cngKey);
        else if (cngKey.AlgorithmGroup.Equals(CngAlgorithmGroup.Rsa))
          testKey = new RSACng(cngKey);
        else
          throw new NotSupportedException(string.Format("Unsupported algorithm: {0}", cngKey.Algorithm));
      }
      Jose.JwsAlgorithm alg;
      if (testKey is RSA)
      {
        alg = Jose.JwsAlgorithm.RS256;
      }
      else if (testKey is ECDsa ecdsaKey)
      {
        switch (ecdsaKey.KeySize)
        {
          case 256:
            alg = Jose.JwsAlgorithm.ES256;
            break;
          case 384:
            alg = Jose.JwsAlgorithm.ES384;
            break;
          case 521: // NB: ES512 uses the P-521/secp521r1 curve which has a key size of 521 bits, this is not a typo...
            alg = Jose.JwsAlgorithm.ES512;
            break;
          default:
            throw new NotSupportedException(string.Format("Unsupported ECDSA key size: {0}", ecdsaKey.KeySize));
        }
      }
      else
      {
        throw new NotSupportedException(string.Format("Unsupported private key type: {0}", key.GetType()));
      }
      return OAuth2_GetAccessTokenFromJWT(session, alg, key, args, flowType, extraHeaders, extraClaims, extraRequestArgs, useAccessToken);
    }

    /// <summary>
    /// Get an access token using the flow "Client Credentials Grant" defined in RFC 6749 Section 4.4 with JWT client credentials signed with a generic algorithm.
    /// </summary>
    /// <param name="session">Ramone session.</param>
    /// <param name="signingAlgorithm">An implementation of ISigningAlgorithm to do the actual signing.</param>
    /// <param name="key">The key used by the signing algorithm.</param>
    /// <param name="args">Assertion arguments.</param>
    /// <param name="useAccessToken">Store the returned access token in session and use that in future requests to the resource server.</param>
    /// <returns></returns>
    public static OAuth2AccessTokenResponse OAuth2_GetAccessTokenFromJWT(
      this ISession session, 
      Jose.JwsAlgorithm alg, 
      object key, 
      AssertionArgs args, 
      bool useAccessToken = true)
    {
      return OAuth2_GetAccessTokenFromJWT(session, alg, key, args, ClientAuthenticationFlowType.Rfc7523Section21, null, null, null, useAccessToken);
    }

    /// <summary>
    /// Get an access token using either the flow "JWTs as Authorization Grants" defined in RFC 7523 Section 2.1 or
    /// "Client Acting on Behalf of Itself" defined in RFC 7521 Section 6.2 with JWT client credentials signed with a generic algorithm.
    /// </summary>
    /// <param name="session">Ramone session.</param>
    /// <param name="alg">An implementation of ISigningAlgorithm to do the actual signing.<</param>
    /// <param name="key">The key used by the signing algorithm.</param>
    /// <param name="args">Assertion arguments.</param>
    /// <param name="flowType">Specify which client authentication flow to use.</param>
    /// <param name="extraHeaders">Optionally specify extra headers in the assertion.</param>
    /// <param name="extraClaims">Optionally specify extra claims in the assertion.</param>
    /// <param name="extraRequestArgs">Optionally specify extra arguments in the POST data of the HTTP request.</param>
    /// <param name="useAccessToken">Store the returned access token in session and use that in future requests to the resource server.</param>
    /// <returns></returns>
    public static OAuth2AccessTokenResponse OAuth2_GetAccessTokenFromJWT(
      this ISession session, 
      Jose.JwsAlgorithm alg, 
      object key, 
      AssertionArgs args,
      ClientAuthenticationFlowType flowType,
      IDictionary<string, object> extraHeaders = null, 
      IDictionary<string, object> extraClaims = null, 
      IDictionary<string, string> extraRequestArgs = null,
      bool useAccessToken = true)
    {
      if (args == null)
        throw new ArgumentNullException(nameof(args));
      if (flowType != ClientAuthenticationFlowType.Rfc7523Section21 && flowType != ClientAuthenticationFlowType.Rfc7521Section62)
        throw new ArgumentOutOfRangeException(nameof(flowType));

      OAuth2Settings settings = GetSettings(session);

      DateTime now = DateTime.UtcNow;
      DateTime issuedAtDate = now.Add(args.IssueTimeOffset);
      DateTime expiresDate = issuedAtDate.Add(args.ExpireTime);
      long issuedAt = issuedAtDate.ToUnixTime();
      long expires = expiresDate.ToUnixTime();

      IEnumerable<KeyValuePair<string, object>> baseClaims = new Dictionary<string,object>()
      {
        {"iss", args.Issuer},
        {"scope", args.Scope},
        {"aud", args.Audience},
        {"sub", args.Subject},
        {"exp", expires},
        {"iat", issuedAt}
      };
      if (extraClaims != null)
        baseClaims = baseClaims.Concat(extraClaims);
      var claims = baseClaims.Where(kv => kv.Value != null).ToDictionary(kv => kv.Key, kv => kv.Value);
      
      string token = Jose.JWT.Encode(claims, key, alg, extraHeaders: extraHeaders);

      NameValueCollection tokenRequestArgs = new NameValueCollection();
      if (flowType == ClientAuthenticationFlowType.Rfc7523Section21)
      {
        tokenRequestArgs["grant_type"] = "urn:ietf:params:oauth:grant-type:jwt-bearer";
        tokenRequestArgs["assertion"] = token;
      }
      else
      {
        tokenRequestArgs["scope"] = args.Scope; // This is nominally optional and redundant, but Microsoft wants it...
        tokenRequestArgs["grant_type"] = "client_credentials";
        tokenRequestArgs["client_assertion_type"] = "urn:ietf:params:oauth:client-assertion-type:jwt-bearer";
        tokenRequestArgs["client_assertion"] = token;
      }
      if (extraRequestArgs != null)
      {
        foreach (var kv in extraRequestArgs)
          tokenRequestArgs[kv.Key] = kv.Value;
      }

      return GetAndStoreAccessToken(session, tokenRequestArgs, useAccessToken);
    }


    public static OAuth2AccessTokenResponse OAuth2_RefreshAccessToken(
      this ISession session, 
      string refreshToken, 
      string scope = null, 
      bool useAccessToken = true)
    {
      OAuth2Settings settings = GetSettings(session);

      NameValueCollection tokenRequestArgs = new NameValueCollection();
      tokenRequestArgs["grant_type"] = "refresh_token";
      tokenRequestArgs["refresh_token"] = refreshToken;
      tokenRequestArgs["scope"] = scope;

      if (settings.ClientAuthenticationMethod == OAuth2Settings.DefaultClientAuthenticationMethods.RequestBody)
      {
        tokenRequestArgs["client_id"] = settings.ClientID;
        tokenRequestArgs["client_secret"] = settings.ClientSecret;
      }

      return GetAndStoreAccessToken(session, tokenRequestArgs, useAccessToken);
    }


    /// <summary>
    /// Does this session have an active access token associated with it?
    /// </summary>
    /// <param name="session">Ramone session.</param>
    /// <returns></returns>
    public static bool OAuth2_HasActiveAccessToken(this ISession session)
    {
      return session.RequestInterceptors.Find("Bearer") != null;
    }


    /// <summary>
    /// Get a copy of the OAuth2 settings (use OAuth2_Configure to change them)
    /// </summary>
    /// <param name="session">Ramone session.</param>
    /// <returns></returns>
    public static OAuth2Settings OAuth2_GetSettings(this ISession session)
    {
      object settings;
      session.Items.TryGetValue(OAuth2SettingsSessionKey, out settings);
      return settings as OAuth2Settings;
    }


    /// <summary>
    /// Get current authorization state.
    /// </summary>
    /// <remarks>The authorization state contains information about active authorization codes,
    /// authorization request state, access token and so on. The state can later on be restored with a
    /// called to OAuth2_RestoreState.</remarks>
    /// <param name="session">Ramone session.</param>
    /// <returns></returns>
    public static OAuth2SessionState OAuth2_GetState(this ISession session)
    {
      object state;
      session.Items.TryGetValue(OAuth2StateSessionKey, out state);
      return state as OAuth2SessionState;
    }


    /// <summary>
    /// Restore authorization state previously obtained from OAuth2_GetState.
    /// </summary>
    /// <param name="session">Ramone session.</param>
    /// <param name="state"></param>
    /// <returns></returns>
    public static ISession OAuth2_RestoreState(this ISession session, OAuth2SessionState state)
    {
      session.Items[OAuth2StateSessionKey] = state;
      if (state.AccessToken != null)
        OAuth2_ActivateAuthorization(session, state.AccessToken, state.TokenType);
      return session;
    }


    internal static OAuth2SessionState OAuth2_GetOrCreateState(this ISession session)
    {
      object obj;
      session.Items.TryGetValue(OAuth2StateSessionKey, out obj);
      OAuth2SessionState state = obj as OAuth2SessionState;
      if (state == null)
      {
        state = new OAuth2SessionState();
        session.Items[OAuth2StateSessionKey] = state;
      }
      return state;
    }


    /// <summary>
    /// Request access token, passing the supplied args to the OAuth2 endpoint.
    /// </summary>
    /// <param name="session"></param>
    /// <param name="args"></param>
    /// <param name="useAccessToken">Store the returned access token in session and use that in future requests to the resource server.</param>
    /// <returns></returns>
    public static OAuth2AccessTokenResponse GetAndStoreAccessToken(ISession session, object args, bool useAccessToken)
    {
      OAuth2Settings settings = GetSettings(session);

      Request request = session.Bind(settings.TokenEndpoint)
                               .AsFormUrlEncoded()
                               .AcceptJson();

      if (settings.ClientAuthenticationMethod == OAuth2Settings.DefaultClientAuthenticationMethods.BasicAuthenticationHeader)
        request = request.BasicAuthentication(settings.ClientID, settings.ClientSecret);

      using (var response = request.AcceptJson().Post<Hashtable>(args))
      {
        OAuth2AccessTokenResponse accessToken = new OAuth2AccessTokenResponse
        {
          access_token = TryGet<string>(response.Body["access_token"]),
          token_type = TryGet<string>(response.Body["token_type"]),
          expires_in = TryGet<long?>(response.Body["expires_in"]),
          refresh_token = TryGet<string>(response.Body["refresh_token"]),
          AllParameters = response.Body
        };

        if (useAccessToken)
        {
          OAuth2SessionState state = session.OAuth2_GetOrCreateState();
          state.AccessToken = accessToken.access_token;
          state.TokenType = ParseAccessTokenType(accessToken.token_type);

          OAuth2_ActivateAuthorization(session, accessToken.access_token, state.TokenType);
        }
        return accessToken;
      }
    }


    private static T TryGet<T>(object v)
    {
      if (v == null)
        return default(T);
      if (v is T)
        return (T)v;
      return default(T);
    }


    /// <summary>
    /// Store access token in session and use it for future requests.
    /// </summary>
    /// <param name="session">Ramone session.</param>
    /// <param name="accessToken">Access token to use.</param>
    /// <param name="tokenType">Token type (so far only "bearer" is supported)</param>
    public static void OAuth2_ActivateAuthorization(ISession session, string accessToken, AccessTokenType tokenType)
    {
      if (tokenType == AccessTokenType.Bearer)
      {
        session.RequestInterceptors.Add("Bearer", new BearerTokenRequestInterceptor(accessToken));
      }
      else
        throw new InvalidOperationException(string.Format("Unknown access token type '{0}' (expected 'bearer')", tokenType));
    }


    private static AccessTokenType ParseAccessTokenType(string tokenType)
    {
      if (string.Equals(tokenType, "bearer", StringComparison.InvariantCultureIgnoreCase))
        return AccessTokenType.Bearer;
      else
        throw new InvalidOperationException(string.Format("Unknown access token type '{0}' (expected 'bearer')", tokenType));
    }


    public static OAuth2Settings GetSettings(ISession session)
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
