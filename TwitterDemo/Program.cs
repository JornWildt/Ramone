using System;
using System.Diagnostics;
using System.IO;
using Ramone;
using Ramone.MediaTypes.FormUrlEncoded;
using Ramone.OAuth1;


namespace TwitterDemo
{
  public class Program
  {
    static IRamoneSession Session { get; set; }

    // Put your own screen name here
    static string TwitterUserScreenName = "JornWildt";


    static void Main(string[] args)
    {
      Setup();

      //ShowTimelineForScreenName_Dynamic(TwitterUserScreenName);
      //Console.WriteLine();
      //ShowTimelineForScreenName_Static(TwitterUserScreenName);

      UpdateUserName();
    }


    static void Setup()
    {
      // Create new session with implicit service
      Session = RamoneConfiguration.NewSession(new Uri("https://api.twitter.com"));

      // Set default request/response media-type to JSON for Twitter.
      // This saves us the hassle of specifying codecs for all the Twitter resource types (Tweet, Timeline, User etc.)
      Session.DefaultRequestMediaType = "application/x-www-form-urlencoded";
      Session.DefaultResponseMediaType = "application/json";

      // Authorize access to Twitter
      AuthorizeTwitterAccess();
    }


    static void AuthorizeTwitterAccess()
    {
      // FIXME: move this to OAuth1 library (and use "*/*" media type when possible)
      // Register codecs for interacting with Twitter.
      // (This is so silly: Twitter returning text/html when it is application/x-www-form-urlencoded.
      // See https://dev.twitter.com/discussions/5662)
      Session.Service.CodecManager.AddCodec<OAuth1Token>("text/html", new FormUrlEncodedSerializerCodec());

      // Get Twitter API keys from file (don't want the secret parts hardcoded in public repository
      TwitterKeys keys = ReadKeys();

      // Configure OAuth1 with the stuff it needs for it's magic
      OAuth1Settings settings = new OAuth1Settings
      {
        ConsumerKey = keys.consumer_key,
        ConsumerSecrect = keys.consumer_secret,
        RequestTokenUrl = new Uri(Session.BaseUri, TwitterApi.OAuthRequestTokenPath),
        AuthorizeUrl = new Uri(Session.BaseUri, TwitterApi.OAuthAuthorizePath),
        AccessTokenUrl = new Uri(Session.BaseUri, TwitterApi.OAuthAccessTokenPath)
      };
      Session.OAuth1Configure(settings);

      // Get temporary credentials from Twitter (request token) and remember it internally
      OAuth1Token requestToken = Session.OAuth1GetRequestToken();

      // Ask user to authorize use of the request token
      Console.WriteLine("Now opening a browser with autorization info. Please follow instructions there.");
      RamoneRequest authorizationRequest = Session.Bind(TwitterApi.OAuthAuthorizePath, requestToken);
      Process.Start(authorizationRequest.Url.AbsoluteUri);

      Console.WriteLine("Please enter Twitter pincode: ");
      string pincode = Console.ReadLine();

      // Get access credentials from Twitter
      Session.OAuth1GetAccessTokenFromRequestToken(pincode);
    }


    // Get timeline for a Twitter user and access it using C# dynamics
    static void ShowTimelineForScreenName_Dynamic(string screenName)
    {
      // Bind user-timeline template to supplied values
      RamoneRequest request = Session.Bind(TwitterApi.UserTimeLineTemplate, new { screen_name = screenName, count = 2 });

      RamoneResponse response = request.Get();

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
    static void ShowTimelineForScreenName_Static(string screenName)
    {
      // Bind user-timeline template to supplied values
      RamoneRequest request = Session.Bind(TwitterApi.UserTimeLineTemplate, new { screen_name = screenName, count = 2 });

      RamoneResponse<Timeline> response = request.Get<Timeline>();

      Timeline timeline = response.Body;

      Console.WriteLine("This is the timeline for {0} using typed access:", screenName);
      Console.WriteLine();
      foreach (Tweet tweet in timeline)
      {
        Console.WriteLine("* [{0}] {1}.", tweet.user.name, tweet.text);
        Console.WriteLine();
      }
    }


    static void UpdateUserName()
    {
      RamoneRequest request = Session.Bind(TwitterApi.UpdateProfileTemplate, new { name = "Jorn Wildt" });
      RamoneResponse response = request.Post(new { });
    }


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
          access_token = keys[2],
          access_token_secret = keys[3]
        };
      }
    }

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
