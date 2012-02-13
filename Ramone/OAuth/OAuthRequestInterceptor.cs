using System.Net;
using Ramone.OAuth.Parameters;


namespace Ramone.OAuth
{
  public class OAuthRequestInterceptor : IRequestInterceptor
  {
    protected string consumer_key { get; set; }

    protected string consumer_secret { get; set; }

    protected string callback { get; set; }

    protected string access_token { get; set; }

    protected string access_token_secret { get; set; }


    protected enum AuthorizationStateType { Started }

    protected AuthorizationStateType AuthorizationState { get; set; }



    public OAuthRequestInterceptor(string consumer_key, string consumer_secret, string callback = null, string access_token = null, string access_token_secret = null)
    {
      this.consumer_key = consumer_key;
      this.consumer_secret = consumer_secret;
      
      this.callback = callback;

      this.access_token = access_token;
      this.access_token_secret = access_token_secret;

      AuthorizationState = AuthorizationStateType.Started;
    }


    public void SetAccessToken(TokenResponse token)
    {
      access_token = token.oauth_token;
      access_token_secret = token.oauth_token_secret;
    }


    #region IRequestInterceptor Members

    public void Intercept(HttpWebRequest request)
    {
      SignatureHelper o = new SignatureHelper();

      string timestamp = o.GenerateTimeStamp();
      string nonce = o.GenerateNonce();

      //timestamp = "1328856925";
      //nonce = "adde2ef436c6430692b1cff5fc5205c1";

      string url;
      string requestParams;

      string signature = o.GenerateSignature(request.RequestUri,
                                              consumer_key,
                                              consumer_secret,
                                              callback,
                                              access_token,
                                              access_token_secret,
                                              request.Method,
                                              timestamp,
                                              nonce,
                                              SignatureHelper.SignatureTypes.HMACSHA1, // FIXME: constructor parameter
                                              out url,
                                              out requestParams);

      string auth = string.Format(@"OAuth 
  oauth_consumer_key=""{0}"", 
  oauth_token=""{1}"", 
  oauth_nonce=""{2}"", 
  oauth_timestamp=""{3}"", 
  oauth_signature_method=""{4}"", 
  oauth_signature=""{5}"", 
  oauth_version=""1.0""",
        consumer_key,
        access_token,
        nonce,
        timestamp,
        "HMAC-SHA1",
        o.UrlEncode(signature));

      if (callback != null)
        auth += string.Format(@", oauth_callback=""{0}""", callback);

      request.Headers["Authorization"] = auth;
    }

    #endregion IRequestInterceptor Members
  }
}
