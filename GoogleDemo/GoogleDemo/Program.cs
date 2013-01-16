using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ramone;
using System.IO;
using Ramone.OAuth2;

namespace GoogleDemo
{
  public class Program
  {
    static ISession Session { get; set; }

    static void Main(string[] args)
    {
      Setup();
    }


    static void Setup()
    {
      // Create new session with implicit service
      Session = RamoneConfiguration.NewSession(new Uri("http://api.google.com"));

      //// Set default request/response media-types to UrlEncoded/JSON for Google.
      //// This saves us the hassle of specifying codecs for all the Google resource types (Tweet, Timeline, User etc.)
      //Session.DefaultRequestMediaType = MediaType.ApplicationFormUrlEncoded;
      //Session.DefaultResponseMediaType = MediaType.ApplicationJson;
    }


    static void AuthorizeGoogleAccess_UsingOutOfBandPincode()
    {
      // Get Google API keys from file (don't want the secret parts hardcoded in public repository
      GoogleKeys keys = ReadKeys();

      // Configure OAuth2 with the stuff it needs for it's magic
      OAuth2Settings settings = new OAuth2Settings
      {
        AuthorizationEndpoint = new Uri("https://accounts.google.com/o/oauth2/auth"),
        ClientID = keys.ClientId,
        ClientSecret = keys.ClientSecret
      };
      Session.OAuth2_Configure(settings);

      // Get temporary credentials from Google (authorization code) and remember it internally
      OAuth2AuthorizationCodeResponse codeResponse = Session.OAuth2_GetAuthorize();

      // Ask user to authorize use of the request token
      Console.WriteLine("Now opening a browser with autorization info. Please follow instructions there.");
      Request authorizationRequest = Session.Bind(GoogleApi.OAuthAuthorizePath, requestToken);
      Process.Start(authorizationRequest.Url.AbsoluteUri);

      Console.WriteLine("Please enter Google pincode: ");
      string pincode = Console.ReadLine();

      if (!string.IsNullOrWhiteSpace(pincode))
      {
        // Get access credentials from Google
        Session.OAuth2GetAccessTokenFromRequestToken(pincode);
      }
    }


    #region Utility stuff for reading keys

    public class GoogleKeys
    {
      public string ClientId { get; set; }
      public string ClientSecret { get; set; }
    }


    static GoogleKeys ReadKeys()
    {
      string[] keys;
      using (TextReader reader = new StreamReader("c:\\tmp\\googlekeys.txt"))
      {
        string keystring = reader.ReadToEnd();
        keys = keystring.Split('|');

        return new GoogleKeys
        {
          ClientId = keys[0],
          ClientSecret = keys[1]
        };
      }
    }

    #endregion
  }
}
