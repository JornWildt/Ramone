using System;


namespace Ramone.OAuth1
{
  public class OAuth1Settings
  {
    public string ConsumerKey { get; set; }

    public string ConsumerSecrect { get; set; }

    public string AccessToken { get; set; }

    public string AccessTokenSecrect { get; set; }

    public Uri RequestTokenUrl { get; set; }

    public Uri AuthorizeUrl { get; set; }

    public Uri AccessTokenUrl { get; set; }

    public string CallbackUrl { get; set; }

    /// <summary>
    /// Set specific Nonce to be used when debugging problems with OAuth1. Do not use normally.
    /// </summary>
    public string Debug_Nonce { get; set; }

    /// <summary>
    /// Set specific timestamp to be used when debugging problems with OAuth1. Do not use normally.
    /// </summary>
    public string Debug_Timestamp { get; set; }


    public OAuth1Settings()
    {
    }


    public OAuth1Settings(OAuth1Settings src)
    {
      ConsumerKey = src.ConsumerKey;
      ConsumerSecrect = src.ConsumerSecrect;
      AccessToken = src.AccessToken;
      AccessTokenSecrect = src.AccessTokenSecrect;
      RequestTokenUrl = src.RequestTokenUrl;
      AuthorizeUrl = src.AuthorizeUrl;
      AccessTokenUrl = src.AccessTokenUrl;
      CallbackUrl = src.CallbackUrl;
      Debug_Nonce = src.Debug_Nonce;
      Debug_Timestamp = src.Debug_Timestamp;
    }
  }
}
