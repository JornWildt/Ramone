using System.Net;


namespace Ramone.OAuth1
{
  public class OAuth1RequestInterceptor : IRequestInterceptor
  {
    public OAuth1Settings Settings { get; set; }

    public bool IsAuthorized { get; set; }

    public IOAuth1Logger Logger { get; set; }


    public OAuth1RequestInterceptor(OAuth1Settings settings)
    {
      Settings = new OAuth1Settings(settings);
    }


    public void SetAccessToken(OAuth1Token token)
    {
      Settings.AccessToken = token.oauth_token;
      Settings.AccessTokenSecrect = token.oauth_token_secret;
    }


    #region IRequestInterceptor Members

    public void HeadersReady(RequestContext context)
    {
      SignatureHelper o = new SignatureHelper(Settings, Logger);
      HttpWebRequest request = context.Request;

      string timestamp = o.GenerateTimeStamp();
      string nonce = o.GenerateNonce();

      if (Settings.Debug_Timestamp != null)
        timestamp = Settings.Debug_Timestamp;

      if (Settings.Debug_Nonce != null)
        nonce = Settings.Debug_Nonce;

      string url;
      string requestParams;

      string signature = o.GenerateSignature(request.RequestUri,
                                             Settings.ConsumerKey,
                                             Settings.ConsumerSecrect,
                                             Settings.CallbackUrl,
                                             Settings.AccessToken,
                                             Settings.AccessTokenSecrect,
                                             request.Method,
                                             timestamp,
                                             nonce,
                                             SignatureTypes.HMACSHA1, // FIXME: constructor parameter
                                             out url,
                                             out requestParams);

      Log("Signature: " + signature);

      string auth = string.Format(@"OAuth 
  oauth_consumer_key=""{0}"", 
  oauth_token=""{1}"", 
  oauth_nonce=""{2}"", 
  oauth_timestamp=""{3}"", 
  oauth_signature_method=""{4}"", 
  oauth_signature=""{5}"", 
  oauth_version=""1.0""",
        Settings.ConsumerKey,
        Settings.AccessToken,
        nonce,
        timestamp,
        "HMAC-SHA1",
        SignatureHelper.UrlEncode(signature));

      if (Settings.CallbackUrl != null)
        auth += string.Format(@", oauth_callback=""{0}""", Settings.CallbackUrl);

      Log("Authorization header: " + auth);

      request.Headers["Authorization"] = auth;
    }

    
    public void DataSent(RequestContext context)
    {
    }

    #endregion IRequestInterceptor Members


    protected void Log(string message)
    {
      if (Logger != null)
        Logger.Log(Settings, message);
    }
  }
}
