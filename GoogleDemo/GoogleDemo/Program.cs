using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using Ramone;
using Ramone.OAuth2;

// This program will access your google account and retreive user name and e-mail

namespace GoogleDemo
{
  public class Program
  {
    static ISession Session { get; set; }

    static void Main(string[] args)
    {
      Console.WriteLine("This program will access your google account and retreive user name and e-mail");
      try
      {
        Setup();
        AuthorizeGoogleAccess_Using_AuthorizationCodeGrantWithPincode();
      }
      catch (WebException ex)
      {
        // Catch web exceptions and stream the content to a file
        using (TextReader reader = new StreamReader(((HttpWebResponse)ex.Response).GetResponseStream()))
        {
          string content = reader.ReadToEnd();
          File.WriteAllText("c:\\tmp\\google-output.html", content);
          Console.WriteLine(content);
          Console.ReadKey();
        }
      }
    }


    static void Setup()
    {
      // Create new session with implicit service
      Session = RamoneConfiguration.NewSession(new Uri("https://www.googleapis.com/oauth2/v1"));
    }


    static void AuthorizeGoogleAccess_Using_AuthorizationCodeGrantWithPincode()
    {
      // Get Google API keys from file (don't want the secret parts hardcoded in public repository
      GoogleKeys keys = ReadKeys();

      // Configure OAuth2 with the stuff it needs for it's magic
      OAuth2Settings settings = new OAuth2Settings
      {
        AuthorizationEndpoint = new Uri("https://accounts.google.com/o/oauth2/auth"),
        TokenEndpoint = new Uri("https://accounts.google.com/o/oauth2/token"),
        ClientID = keys.ClientId,
        ClientSecret = keys.ClientSecret,
        RedirectUri = new Uri("urn:ietf:wg:oauth:2.0:oob")
      };
      Session.OAuth2_Configure(settings);

      // Get temporary credentials from Google (authorization code) and remember it internally
      OAuth2AuthorizationRedirect redirectResponse = Session.OAuth2_AuthorizeWithRedirect("https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/userinfo.profile");

      // Ask user to authorize use of the request token
      Console.WriteLine("Now opening a browser with autorization info. Please follow instructions there.");
      Process.Start(redirectResponse.Location.AbsoluteUri);

      Console.WriteLine("\nPlease enter Google authorization code from browser authorization: ");
      string authorizationCode = Console.ReadLine();

      if (!string.IsNullOrWhiteSpace(authorizationCode))
      {
        // Get access credentials from Google
        OAuth2AccessTokenResponse token = Session.OAuth2_GetAccessTokenFromAuthorizationCode(authorizationCode);

        using (var response = Session.Bind("userinfo").AcceptJson().Get<dynamic>())
        {
          var body = response.Body;
          Console.WriteLine("\nRESULT:");
          Console.WriteLine("User name: " + body.name);
          Console.WriteLine("E-mail: " + body.email);
        }
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
