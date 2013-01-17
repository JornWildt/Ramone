using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using Ramone;
using Ramone.OAuth2;

// This program will access your google account and retreive user name and e-mail

// Before using this program you MUST require a set of client credentials from Google's API console and 
// store these in a local file (see filename below).

namespace GoogleDemo
{
  public class Program
  {
    // Define scope for user info request to Google
    const string Scope = "https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/userinfo.profile";

    // Base URL for Google API
    const string GoogleAPIBaseUrl = "https://www.googleapis.com/oauth2/v1";

    // Google's OAuth2 Authorization Endpoint
    const string AuthorizationEndpointUrl = "https://accounts.google.com/o/oauth2/auth";

    // Google's OAuth2 Token Endpoint
    const string TokenEndpointUrl = "https://accounts.google.com/o/oauth2/token";

    // API keys filename
    const string APIKeysFilename = "c:\\tmp\\googlekeys.txt";

    
    static bool UsePincode = false;

    static ISession Session { get; set; }

    
    static void Main(string[] args)
    {
      Console.WriteLine("This program will access your google account and retreive user name and e-mail.\n");

      try
      {
        Setup();
        if (UsePincode)
          AuthorizeGoogleAccess_Using_AuthorizationCodeGrantWithPincode();
        else
          AuthorizeGoogleAccess_Using_AuthorizationCodeGrantWithRedirect();
        ReadUserInfo();
      }
      catch (WebException ex)
      {
        // Catch web exceptions and stream the content to a file
        using (TextReader reader = new StreamReader(((HttpWebResponse)ex.Response).GetResponseStream()))
        {
          string content = reader.ReadToEnd();
          Console.WriteLine(content);
          File.WriteAllText("c:\\tmp\\google-output.html", content);
          Console.ReadKey();
        }
      }
    }


    static void ReadUserInfo()
    {
      using (var response = Session.Bind("userinfo").AcceptJson().Get<dynamic>())
      {
        var body = response.Body;
        Console.WriteLine("\nRESULT:");
        Console.WriteLine("User name: " + body.name);
        Console.WriteLine("E-mail: " + body.email);
      }
    }


    static void Setup()
    {
      // Create new session with implicit service
      Session = RamoneConfiguration.NewSession(new Uri(GoogleAPIBaseUrl));

      // Get Google API keys from file (don't want the secret parts hardcoded in public repository
      GoogleKeys keys = ReadKeys();

      // Configure OAuth2 with the stuff it needs for it's magic
      OAuth2Settings settings = new OAuth2Settings
      {
        AuthorizationEndpoint = new Uri(AuthorizationEndpointUrl),
        TokenEndpoint = new Uri(TokenEndpointUrl),
        ClientID = keys.ClientId,
        ClientSecret = keys.ClientSecret        
      };
      
      // Instruct Google to display pincode (for copy/paste) or do a redirect with authorization code in URL
      if (UsePincode)
        settings.RedirectUri = new Uri("urn:ietf:wg:oauth:2.0:oob");
      else
        settings.RedirectUri = new Uri("http://localhost");

      Session.OAuth2_Configure(settings);
    }


    static void AuthorizeGoogleAccess_Using_AuthorizationCodeGrantWithPincode()
    {
      // Get temporary credentials from Google (authorization code) and use it for initial URL
      Uri authorizationUrl = Session.OAuth2_GetAuthorizationRequestUrl(Scope);

      // Ask user to authorize use of the request token
      Console.WriteLine("Now opening a browser with autorization info. Please follow instructions there.");
      Process.Start(authorizationUrl.AbsoluteUri);

      Console.WriteLine("\nPlease enter Google authorization code from browser authorization: ");
      string authorizationCode = Console.ReadLine();

      if (!string.IsNullOrWhiteSpace(authorizationCode))
      {
        // Get access credentials from Google
        OAuth2AccessTokenResponse token = Session.OAuth2_GetAccessTokenFromAuthorizationCode(authorizationCode);
      }
    }


    static void AuthorizeGoogleAccess_Using_AuthorizationCodeGrantWithRedirect()
    {
      // Get temporary credentials from Google (authorization code) and use it for initial URL
      Uri authorizationUrl = Session.OAuth2_GetAuthorizationRequestUrl(Scope);

      // Ask user to authorize use of the request token
      Console.WriteLine("Now opening a browser with autorization info. Please follow instructions there.");
      Process.Start(authorizationUrl.AbsoluteUri);

      Console.WriteLine("\nIn the end you will be redirected to 'localhost' which most probably");
      Console.WriteLine("does not contain any meaningful content.");

      Console.WriteLine("\nPlease copy URL from browser and paste it here: ");
      string redirectUrl = Console.ReadLine();

      string authorizationCode = Session.OAuth2_GetAuthorizationCodeFromRedirectUrl(redirectUrl);

      if (!string.IsNullOrWhiteSpace(redirectUrl))
      {
        // Extract authorization code from redirect URL

        // Get access credentials from Google
        OAuth2AccessTokenResponse token = Session.OAuth2_GetAccessTokenFromAuthorizationCode(authorizationCode);
      }
    }


    #region Utility stuff for reading keys

    public class GoogleKeys
    {
      public string ClientId { get; set; }
      public string ClientSecret { get; set; }
    }


    // Keys must be stored as one line of text containing: client-id|client-secret
    static GoogleKeys ReadKeys()
    {
      string[] keys;
      using (TextReader reader = new StreamReader(APIKeysFilename))
      {
        string keystring = reader.ReadLine();
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
