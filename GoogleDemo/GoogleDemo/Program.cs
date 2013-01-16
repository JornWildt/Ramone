using System;
using System.IO;
using Ramone;
using Ramone.OAuth2;
using System.Net;
using System.Diagnostics;


namespace GoogleDemo
{
  public class Program
  {
    static ISession Session { get; set; }

    static void Main(string[] args)
    {
      try
      {
        Setup();
        AuthorizeGoogleAccess_UsingOutOfBandPincode();
      }
      catch (Ramone.NotAuthorizedException ex)
      {
        WebException wex = (WebException)ex.InnerException;
        using (TextReader reader = new StreamReader(((HttpWebResponse)wex.Response).GetResponseStream()))
        {
          string content = reader.ReadToEnd();
          File.WriteAllText("c:\\tmp\\google-output.html", content);
          Console.WriteLine(content);
          Console.ReadKey();
        }
      }
      catch (WebException ex)
      {
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

      Console.WriteLine("Please enter Google authorization code: ");
      string authorizationCode = Console.ReadLine();

      if (!string.IsNullOrWhiteSpace(authorizationCode))
      {
        // Get access credentials from Google
        OAuth2AccessTokenResponse token = Session.OAuth2_GetAccessTokenFromAuthorizationCode(authorizationCode);

        using (var response = Session.Bind("userinfo").AcceptJson().Get<dynamic>())
        {
          var body = response.Body;
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
