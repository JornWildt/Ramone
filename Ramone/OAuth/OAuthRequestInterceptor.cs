using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Ramone.OAuth
{
  public class OAuthRequestInterceptor : IRequestInterceptor
  {
    protected string consumer_key { get; set; }

    protected string consumer_secret { get; set; }


    public OAuthRequestInterceptor(string consumer_key, string consumer_secret)
    {
      this.consumer_key = consumer_key;
      this.consumer_secret = consumer_secret;
    }


    #region IRequestInterceptor Members

    public void Intercept(HttpWebRequest request)
    {
      string[] keys;
      using (TextReader reader = new StreamReader("c:\\tmp\\twitterkeys.txt"))
      {
        string keystring = reader.ReadToEnd();
        keys = keystring.Split('|');
      }

      SignatureHelper o = new SignatureHelper();

      string consumer_key = keys[0];
      string consumer_secret = keys[1];

      string access_token = keys[2];
      string access_token_secret = keys[3];

      string timestamp = o.GenerateTimeStamp();
      string nonce = o.GenerateNonce();

      //timestamp = "1328824349";
      //nonce = "c54be2858d15ba0e61f27353729e13d6";

      string url;
      string requestParams;

      string signature = o.GenerateSignature(request.RequestUri,
                                              consumer_key,
                                              consumer_secret,
                                              access_token,
                                              access_token_secret,
                                              request.Method,
                                              timestamp,
                                              nonce,
                                              SignatureHelper.SignatureTypes.HMACSHA1,
                                              out url,
                                              out requestParams);

      string auth = string.Format(@"OAuth oauth_consumer_key=""{1}"", oauth_nonce=""{3}"", oauth_signature=""{6}"", oauth_signature_method=""{5}"", oauth_timestamp=""{4}"", oauth_token=""{2}"", oauth_version=""1.0""",

      url,
      consumer_key,
      access_token,
      nonce,
      timestamp,
      "HMAC-SHA1",
      o.UrlEncode(signature));

      request.Headers["Authorization"] = auth;
    }

    #endregion IRequestInterceptor Members
  }
}
