using System;
using System.Diagnostics;
using System.IO;
using Ramone;
using Ramone.OAuth1;


namespace TwitterDemo
{
  // Demo of Ramone used as a Twitter client that shows timelines and posts updates.

  // In order to run this program you must first obtain pair of OAuth1 API consumer_key and consumer_secret from Twitter.
  // Put these in a file c:\tmp\twitterkeys.txt formated as one line of "consumer_key|consumer_secret"

  public class Program
  {
    static ISession Session { get; set; }


    // Put a screen name here for showing timeline
    static string TwitterUserScreenName = "JornWildt";


    static void Main(string[] args)
    {
      Setup();

      // Read-only, non-authorized, operations

      ShowTimelineForScreenName_Dynamic(TwitterUserScreenName);
      Console.WriteLine();
      ShowTimelineForScreenName_Typed(TwitterUserScreenName);

      // Authorize access to Twitter
      AuthorizeTwitterAccess_UsingOutOfBandPincode();

      // Updating, authorized, operations

      if (Session.OAuth1IsAuthorized())
      {
        // I find this operation less annoying when testing - it doesn't spam my followers with test messages
        //UpdateUserName("Jønke");

        //PostTweet_Dynamic();
        //PostTweet_Typed();
      }
    }


    static void Setup()
    {
      // Create new session with implicit service
      Session = RamoneConfiguration.NewSession(new Uri("https://api.twitter.com"));

      // Set default request/response media-types to UrlEncoded/JSON for Twitter.
      // This saves us the hassle of specifying codecs for all the Twitter resource types (Tweet, Timeline, User etc.)
      Session.DefaultRequestMediaType = MediaType.ApplicationFormUrlEncoded;
      Session.DefaultResponseMediaType = MediaType.ApplicationJson;
    }


    static void AuthorizeTwitterAccess_UsingOutOfBandPincode()
    {
      // Get Twitter API keys from file (don't want the secret parts hardcoded in public repository
      TwitterKeys keys = ReadKeys();

      // Configure OAuth1 with the stuff it needs for it's magic
      OAuth1Settings settings = new OAuth1Settings
      {
        ConsumerKey = keys.consumer_key,
        ConsumerSecrect = keys.consumer_secret,
        RequestTokenUrl = new Uri(Session.BaseUri, TwitterApi.OAuthRequestTokenPath),
        AuthorizeUrl = new Uri(Session.BaseUri, TwitterApi.OAuthAuthorizePath),
        AccessTokenUrl = new Uri(Session.BaseUri, TwitterApi.OAuthAccessTokenPath),
        CallbackUrl = "oob" // oob = Out Of Band
      };
      Session.OAuth1Configure(settings);

      // Attach a logger after settings has been assigned (use debugger for breakpoints inside it)
      Session.OAuth1Logger(new OAuth1Logger());

      // Get temporary credentials from Twitter (request token) and remember it internally
      OAuth1Token requestToken = Session.OAuth1GetRequestToken();

      // Ask user to authorize use of the request token
      Console.WriteLine("Now opening a browser with autorization info. Please follow instructions there.");
      Request authorizationRequest = Session.Bind(TwitterApi.OAuthAuthorizePath, requestToken);
      Process.Start(authorizationRequest.Url.AbsoluteUri);

      Console.WriteLine("Please enter Twitter pincode: ");
      string pincode = Console.ReadLine();

      if (!string.IsNullOrWhiteSpace(pincode))
      {
        // Get access credentials from Twitter
        Session.OAuth1GetAccessTokenFromRequestToken(pincode);
      }
    }


    // Get timeline for a Twitter user and access it using C# dynamics
    static void ShowTimelineForScreenName_Dynamic(string screenName)
    {
      // Bind user-timeline template to supplied values
      Request request = Session.Bind(TwitterApi.UserTimeLinePath, new { screen_name = screenName, count = 2 });

      // GET response
      Response response = request.Get();

      dynamic timeline = response.Body;

      Console.WriteLine("This is the timeline for {0} using C# dynamics:", screenName);
      Console.WriteLine();

      foreach (dynamic tweet in timeline)
      {
        Console.WriteLine("* [{0}] {1}.", tweet.user.name, tweet.text);
        Console.WriteLine();
      }
    }


    // Get timeline for a Twitter user and access it using typed types
    static void ShowTimelineForScreenName_Typed(string screenName)
    {
      // Bind user-timeline template to supplied values
      Request request = Session.Bind(TwitterApi.UserTimeLinePath, new { screen_name = screenName, count = 2 });

      // GET statically typed response
      Response<Timeline> response = request.Get<Timeline>();

      Timeline timeline = response.Body;

      Console.WriteLine("This is the timeline for {0} using typed access:", screenName);
      Console.WriteLine();
      foreach (Tweet tweet in timeline)
      {
        Console.WriteLine("* [{0}] {1}.", tweet.user.name, tweet.text);
        Console.WriteLine();
      }
    }


    static void PostTweet_Dynamic()
    {
      // Insert GUID because Twitter denies access to identical tweets
      string message = "A dynamic tweet from the Ramone client [" + Guid.NewGuid() + "]";
      Session.Bind(TwitterApi.StatusesUpdate, new { status = message }).Post();
      Console.WriteLine("Posted update using dynamic object parameters.");
    }


    static void PostTweet_Typed()
    {
      // Insert GUID because Twitter denies access to identical tweets
      string message = "A typed tweet from the Ramone client [" + Guid.NewGuid() + "]";
      StatusUpdate update = new StatusUpdate { status = message };
      Session.Bind(TwitterApi.StatusesUpdate, update).Post();
      Console.WriteLine("Posted update using typed object parameters.");
    }


    static void UpdateUserName(string name)
    {
      Request request = Session.Bind(TwitterApi.UpdateProfilePath, new { name = name });
      Response response = request.Post();
    }


    #region Utility stuff for reading Twitter keys

    class TwitterKeys
    {
      public string consumer_key;
      public string consumer_secret;

      public string access_token;
      public string access_token_secret;
    }

    
    static TwitterKeys ReadKeys()
    {
      string[] keys;
      using (TextReader reader = new StreamReader("c:\\tmp\\twitterkeys.txt"))
      {
        string keystring = reader.ReadToEnd();
        keys = keystring.Split('|');

        return new TwitterKeys
        {
          consumer_key = keys[0],
          consumer_secret = keys[1],
          access_token = keys.Length > 2 ? keys[2] : null,
          access_token_secret = keys.Length > 3 ? keys[3] : null
        };
      }
    }

    #endregion


#if false
    // Ignore this for now ...

    // Example of problems with JsonFx. See https://github.com/jsonfx/jsonfx/issues/19

    static void JsonReadFailure()
    {
      HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.twitter.com/1/statuses/user_timeline.json?screen_name=JornWildt&count=2");
      HttpWebResponse response = (HttpWebResponse)request.GetResponse();

      JsonReader jsr = new JsonReader();
      using (TextReader reader = new StreamReader(response.GetResponseStream()))
      {
#if true
        dynamic result = jsr.Read(reader);
#else
        string json = reader.ReadToEnd();
        dynamic result = jsr.Read(json);
#endif

        Console.WriteLine("Number of tweets = {0}.", result.Length);
      }
    }
#endif
  }
}
