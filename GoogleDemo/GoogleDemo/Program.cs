using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Ramone;
using Ramone.OAuth2;
using Ramone.Utility.JsonWebToken;


// This program will access your google account and retreive user name and e-mail

// Before using this program you MUST require either A) a set of client credentials from Google's API console and 
// store these in a local file (see filename below), or B) require a signing certificate from the same place.

namespace GoogleDemo
{
  public class Program
  {
    #region Stuff to be modifed before running

    // *** Assign Google API keys here (get this from the Google API console)
    // API keys filename (must contain one line with: username|password)
    const string APIKeysFilename = "c:\\tmp\\googlekeys.txt";

    // *** Assign API certificate here (get this from the Google API console)
    // Certificate file containing keys for RSA signing of JWT
    const string JWT_CertificatePath = "C:\\Jørn\\Google API - Elfisk - Private key (notasecret).p12";

    // *** Assign JWT issuer here (get this from the Google API console)
    const string JWT_Issuer = "285324442069-2f2ptfikn22ojv5min3ns51tsqoavhuf@developer.gserviceaccount.com";

    #endregion


    #region Hard coded stuff for Google

    // Define scope for user info request to Google
    const string Scope = "https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/userinfo.profile";

    // Base URL for Google API
    const string GoogleAPIBaseUrl = "https://www.googleapis.com/oauth2/v1";

    // Google's OAuth2 Authorization Endpoint
    const string AuthorizationEndpointUrl = "https://accounts.google.com/o/oauth2/auth";

    // Google's OAuth2 Token Endpoint
    const string TokenEndpointUrl = "https://accounts.google.com/o/oauth2/token";

    #endregion


    enum AccessType { PinCode, Redirect, JWT }

    static AccessType SelectedAccessType = AccessType.PinCode;

    static ISession Session { get; set; }

    
    static void Main(string[] args)
    {
      SelectAuthorizationMethod();

      try
      {
        Setup();
        Authorize();
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

    
    private static void SelectAuthorizationMethod()
    {
      Console.WriteLine("This program will access your google account and retreive user name and e-mail.\n");
      Console.WriteLine("");
      Console.WriteLine("Select one of the following authentication techniques:");
      Console.WriteLine(" 1: Get a pin code from Google.");
      Console.WriteLine(" 2: Use redirection URL.");
      Console.WriteLine(" 3: Use JsonWebToken (JWT) signed with private RSA key file.");
      string input = Console.ReadLine().Trim();
      if (input == "1")
        SelectedAccessType = AccessType.PinCode;
      else if (input == "2")
        SelectedAccessType = AccessType.Redirect;
      else if (input == "3")
        SelectedAccessType = AccessType.JWT;
      else
      {
        Console.WriteLine("Unknown input");
        Environment.Exit(1);
      }
    }


    static void Setup()
    {
      // Create new session with implicit service
      Session = RamoneConfiguration.NewSession(new Uri(GoogleAPIBaseUrl));

      // Get Google API keys from file (don't want the secret parts hardcoded in public repository)
      GoogleKeys keys = new GoogleKeys();
      if (SelectedAccessType != AccessType.JWT)
        keys = ReadKeys();

      // Configure OAuth2 with the stuff it needs for it's magic
      OAuth2Settings settings = new OAuth2Settings
      {
        AuthorizationEndpoint = new Uri(AuthorizationEndpointUrl),
        TokenEndpoint = new Uri(TokenEndpointUrl),
        ClientID = keys.ClientId,
        ClientSecret = keys.ClientSecret,
        ClientAuthenticationMethod = OAuth2Settings.DefaultClientAuthenticationMethods.RequestBody
      };

      if (SelectedAccessType == AccessType.PinCode)
        settings.RedirectUri = new Uri("urn:ietf:wg:oauth:2.0:oob");
      else if (SelectedAccessType == AccessType.Redirect)
        settings.RedirectUri = new Uri("http://localhost");
      else
        settings.ClientAuthenticationMethod = OAuth2Settings.DefaultClientAuthenticationMethods.None;

      Session.OAuth2_Configure(settings);
    }


    private static void Authorize()
    {
      if (SelectedAccessType == AccessType.PinCode)
        AuthorizeGoogleAccess_Using_AuthorizationCodeGrantWithPincode();
      else if (SelectedAccessType == AccessType.Redirect)
        AuthorizeGoogleAccess_Using_AuthorizationCodeGrantWithRedirect();
      else
        AuthorizeGoogleAccess_Using_JWT();
    }


    static void ReadUserInfo()
    {
      Console.WriteLine("Reading user information from Google");
      using (var response = Session.Bind("userinfo").AcceptJson().Get<dynamic>())
      {
        var body = response.Body;
        Console.WriteLine("\nRESULT:");
        Console.WriteLine("User name: " + body.name);
        Console.WriteLine("E-mail: " + body.email);
      }
    }


    #region Authorization methods

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


    static void AuthorizeGoogleAccess_Using_JWT()
    {
      Console.WriteLine("Loading certificate from " + JWT_CertificatePath);

      // Load certificate from file and get a crypto service provider for SHA256 signing
      X509Certificate2 certificate = new X509Certificate2(JWT_CertificatePath, "notasecret", X509KeyStorageFlags.Exportable);
      using (RSACryptoServiceProvider cp = (RSACryptoServiceProvider)certificate.PrivateKey)
      {
        // Create new crypto service provider that supports SHA256 (and don't ask me why the first one doesn't)
        CspParameters cspParam = new CspParameters
        {
          KeyContainerName = cp.CspKeyContainerInfo.KeyContainerName,
          KeyNumber = cp.CspKeyContainerInfo.KeyNumber == KeyNumber.Exchange ? 1 : 2
        };

        using (var aes_csp = new RSACryptoServiceProvider(cspParam) { PersistKeyInCsp = false })
        {
          // Parameters for JWT creation
          AssertionArgs args = new AssertionArgs
          {
            Algorithm = Algorithms.RSASHA256,
            Audience = TokenEndpointUrl,
            Issuer = JWT_Issuer,
            Scope = Scope
          };

          Console.WriteLine("Authorizing with Google");
          OAuth2AccessTokenResponse token = Session.OAuth2_GetAccessTokenFromJWT_RSASHA256(aes_csp, args);
        }
      }
    }

    #endregion


    #region Utility stuff for reading keys

    public class GoogleKeys
    {
      public string ClientId { get; set; }
      public string ClientSecret { get; set; }
    }


    // Keys must be stored as one line of text containing: client-id|client-secret
    static GoogleKeys ReadKeys()
    {
      Console.WriteLine("Loading Google API keys from " + APIKeysFilename);
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
