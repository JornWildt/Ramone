using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using CuttingEdge.Conditions;


/// <summary>
/// OAuth utility originally from http://code.google.com/p/oauth/
/// </summary>
namespace Ramone.OAuth1
{
  /// <summary>
  /// Provides a predefined set of algorithms that are supported officially by the protocol
  /// </summary>
  public enum SignatureTypes
  {
    HMACSHA1,
    PLAINTEXT,
    RSASHA1
  }


  public class SignatureHelper
  {
    protected OAuth1Settings Settings { get; set; }

    protected IOAuth1Logger Logger { get; set; }


    protected const string OAuthVersion = "1.0";
    
    protected const string OAuthParameterPrefix = "oauth_";


    //
    // List of known and used oauth parameters' names
    //        
    public const string OAuthConsumerKeyKey = "oauth_consumer_key";
    public const string OAuthCallbackKey = "oauth_callback";
    public const string OAuthVersionKey = "oauth_version";
    public const string OAuthSignatureMethodKey = "oauth_signature_method";
    public const string OAuthSignatureKey = "oauth_signature";
    public const string OAuthTimestampKey = "oauth_timestamp";
    public const string OAuthNonceKey = "oauth_nonce";
    public const string OAuthTokenKey = "oauth_token";
    public const string OAuthTokenSecretKey = "oauth_token_secret";

    public const string HMACSHA1SignatureType = "HMAC-SHA1";
    public const string PlainTextSignatureType = "PLAINTEXT";
    public const string RSASHA1SignatureType = "RSA-SHA1";

    protected const string unreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

    protected Random random = new Random();

    private static Encoding Encoder8bit = Encoding.GetEncoding("iso-8859-1");


    public SignatureHelper(OAuth1Settings settings, IOAuth1Logger logger)
    {
      Settings = settings;
      Logger = logger;
    }


    /// <summary>
    /// Generate the signature base that is used to produce the signature
    /// </summary>
    /// <param name="url">The full url that needs to be signed including its non OAuth url parameters</param>
    /// <param name="consumerKey">The consumer key</param>        
    /// <param name="token">The token, if available. If not available pass null or an empty string</param>
    /// <param name="callback">OAuth callback value.</param>
    /// <param name="httpMethod">The http method used. Must be a valid HTTP method verb (POST,GET,PUT, etc)</param>
    /// <param name="signatureType">The signature type. To use the default values use <see cref="SignatureTypes">OAuthBase.SignatureTypes</see>.</param>
    /// <param name="nonce"></param>
    /// <param name="normalizedRequestParameters"></param>
    /// <param name="normalizedUrl"></param>
    /// <param name="timeStamp"></param>
    /// <returns>The signature base</returns>
    public string GenerateSignatureBase(Uri url, string consumerKey, string callback, string token, string httpMethod, string timeStamp, string nonce, string signatureType, out string normalizedUrl, out string normalizedRequestParameters)
    {
      Condition.Requires(consumerKey, "consumerKey").IsNotNullOrEmpty();
      Condition.Requires(httpMethod, "httpMethod").IsNotNullOrEmpty();
      Condition.Requires(signatureType, "signatureType").IsNotNullOrEmpty();

      if (token == null)
        token = string.Empty;

      normalizedUrl = null;
      normalizedRequestParameters = null;

      List<QueryParameter> parameters = GetQueryParameters(url.Query);
      parameters.Add(new QueryParameter(OAuthVersionKey, OAuthVersion));
      parameters.Add(new QueryParameter(OAuthNonceKey, nonce));
      parameters.Add(new QueryParameter(OAuthTimestampKey, timeStamp));
      parameters.Add(new QueryParameter(OAuthSignatureMethodKey, signatureType));
      parameters.Add(new QueryParameter(OAuthConsumerKeyKey, consumerKey));
      parameters.Add(new QueryParameter(OAuthTokenKey, token));

      if (callback != null)
        parameters.Add(new QueryParameter(OAuthCallbackKey, callback));

      normalizedUrl = string.Format("{0}://{1}", url.Scheme, url.Host);
      if (!((url.Scheme == "http" && url.Port == 80) || (url.Scheme == "https" && url.Port == 443)))
      {
        normalizedUrl += ":" + url.Port;
      }
      normalizedUrl += url.AbsolutePath;
      normalizedRequestParameters = NormalizeRequestParameters(parameters);

      StringBuilder signatureBase = new StringBuilder();
      signatureBase.AppendFormat("{0}&", httpMethod.ToUpper());
      signatureBase.AppendFormat("{0}&", UrlEncode(normalizedUrl));
      signatureBase.AppendFormat("{0}", UrlEncode(normalizedRequestParameters));

      return signatureBase.ToString();
    }


    /// <summary>
    /// Generate the signature value based on the given signature base and hash algorithm
    /// </summary>
    /// <param name="signatureBase">The signature based as produced by the GenerateSignatureBase method or by any other means</param>
    /// <param name="hash">The hash algorithm used to perform the hashing. If the hashing algorithm requires initialization or a key it should be set prior to calling this method</param>
    /// <returns>A base64 string of the hash value</returns>
    public string GenerateSignatureUsingHash(string signatureBase, HashAlgorithm hash)
    {
      return ComputeHash(hash, signatureBase);
    }



    /// <summary>
    /// Generates a signature using the specified signatureType 
    /// </summary>		
    /// <param name="url">The full url that needs to be signed including its non OAuth url parameters</param>
    /// <param name="consumerKey">The consumer key</param>
    /// <param name="consumerSecret">The consumer seceret</param>
    /// <param name="token">The token, if available. If not available pass null or an empty string</param>
    /// <param name="tokenSecret">The token secret, if available. If not available pass null or an empty string</param>
    /// <param name="httpMethod">The http method used. Must be a valid HTTP method verb (POST,GET,PUT, etc)</param>
    /// <param name="signatureType">The type of signature to use</param>
    /// <param name="callback"></param>
    /// <param name="nonce"></param>
    /// <param name="normalizedRequestParameters"></param>
    /// <param name="normalizedUrl"></param>
    /// <param name="timeStamp"></param>
    /// <returns>A base64 string of the hash value</returns>
    public string GenerateSignature(Uri url, string consumerKey, string consumerSecret, string callback, string token, string tokenSecret, string httpMethod, string timeStamp, string nonce, SignatureTypes signatureType, out string normalizedUrl, out string normalizedRequestParameters)
    {
      normalizedUrl = null;
      normalizedRequestParameters = null;

      switch (signatureType)
      {
        case SignatureTypes.PLAINTEXT: // FIXME: Is this correct?
          return HttpUtility.UrlEncode(string.Format("{0}&{1}", consumerSecret, tokenSecret));
        case SignatureTypes.HMACSHA1:
          string signatureBase = GenerateSignatureBase(url, consumerKey, callback, token, httpMethod, timeStamp, nonce, HMACSHA1SignatureType, out normalizedUrl, out normalizedRequestParameters);
          Log("Signaturebase: " + signatureBase);

          string key = string.Format("{0}&{1}", UrlEncode(Encoder8bit.GetString(Encoding.UTF8.GetBytes(consumerSecret ?? ""))),
                                                UrlEncode(Encoder8bit.GetString(Encoding.UTF8.GetBytes(tokenSecret ?? ""))));
          Log("Signature key: " + key);

          HMACSHA1 hmacsha1 = new HMACSHA1();
          hmacsha1.Key = Encoder8bit.GetBytes(key);

          return GenerateSignatureUsingHash(signatureBase, hmacsha1);
        case SignatureTypes.RSASHA1:
          throw new NotImplementedException();
        default:
          throw new ArgumentException("Unknown signature type", "signatureType");
      }
    }


    /// <summary>
    /// Generate the timestamp for the signature        
    /// </summary>
    /// <returns></returns>
    public virtual string GenerateTimeStamp()
    {
      // Default implementation of UNIX time of the current UTC time
      TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
      return Convert.ToInt64(ts.TotalSeconds).ToString();
    }


    /// <summary>
    /// Generate a nonce
    /// </summary>
    /// <returns></returns>
    public virtual string GenerateNonce()
    {
      // Just a simple implementation of a random number between 123400 and 9999999
      return random.Next(123400, 9999999).ToString();
    }


    /// <summary>
    /// Helper function to compute a hash value
    /// </summary>
    /// <param name="hashAlgorithm">The hashing algorihtm used. If that algorithm needs some initialization, like HMAC and its derivatives, they should be initialized prior to passing it to this function</param>
    /// <param name="data">The data to hash</param>
    /// <returns>a Base64 string of the hash value</returns>
    public string ComputeHash(HashAlgorithm hashAlgorithm, string data)
    {
      Condition.Requires(hashAlgorithm, "hashAlgorithm").IsNotNull();
      Condition.Requires(data, "data").IsNotNullOrEmpty();

      byte[] dataBuffer = System.Text.Encoding.ASCII.GetBytes(data);
      byte[] hashBytes = hashAlgorithm.ComputeHash(dataBuffer);

      return Convert.ToBase64String(hashBytes);
    }


    /// <summary>
    /// Internal function to cut out all non oauth query string parameters (all parameters not begining with "oauth_")
    /// </summary>
    /// <param name="query">The query string part of the Url</param>
    /// <returns>A list of QueryParameter each containing the parameter name and value</returns>
    public List<QueryParameter> GetQueryParameters(string query)
    {
      if (query.StartsWith("?"))
      {
        query = query.Remove(0, 1);
      }

      NameValueCollection parameters = HttpUtility.ParseQueryString(query);

      return new List<QueryParameter>(
                   parameters.Cast<string>()
                   .Where(key => !key.StartsWith(OAuthParameterPrefix))
                   .Select(key => new QueryParameter(key, parameters[key])));
    }


    /// <summary>
    /// This is a different Url Encode implementation since the default .NET one outputs the percent encoding in lower case.
    /// While this is not a problem with the percent encoding spec, it is used in upper case throughout OAuth
    /// </summary>
    /// <param name="value">The value to Url encode</param>
    /// <returns>Returns a Url encoded string</returns>
    public static string UrlEncode(string value)
    {
      StringBuilder result = new StringBuilder();

      foreach (char symbol in value)
      {
        if (unreservedChars.IndexOf(symbol) != -1)
        {
          result.Append(symbol);
        }
        else
        {
          result.Append('%' + String.Format("{0:X2}", (int)symbol));
        }
      }

      return result.ToString();
    }


    /// <summary>
    /// Normalizes the request parameters according to the spec
    /// </summary>
    /// <param name="parameters">The list of parameters already sorted</param>
    /// <returns>a string representing the normalized parameters</returns>
    public string NormalizeRequestParameters(List<QueryParameter> parameters)
    {
      parameters.Sort(new QueryParameterComparer());

      StringBuilder sb = new StringBuilder();
      QueryParameter p = null;
      for (int i = 0; i < parameters.Count; i++)
      {
        p = parameters[i];

        if (i > 0)
          sb.Append("&");

        sb.AppendFormat("{0}={1}", p.Name, p.Value);
      }

      return sb.ToString();
    }


    protected void Log(string message)
    {
      if (Logger != null)
        Logger.Log(Settings, message);
    }
  }
}
