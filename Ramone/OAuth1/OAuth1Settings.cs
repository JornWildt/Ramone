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
    }
  }
}
