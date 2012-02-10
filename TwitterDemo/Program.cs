using System;
using Ramone;
using System.Net;
using Ramone.Utility;
using Ramone.OAuth;
using System.IO;
using Ramone.OAuth.Parameters;
using Ramone.MediaTypes;


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

      AuthorizeTwitterAccess();
    }


    static void AuthorizeTwitterAccess()
    {
      Session.Service.CodecManager.AddCodec<string>("text/html", new StringCodec());

      TwitterKeys keys = ReadKeys();
      Session.OAuth1Start(keys.consumer_key, keys.consumer_secret, "oob");//, keys.access_token, keys.access_token_secret);

      //TokenResponse oauthResponse = Session.Bind(TwitterApi.OAuthRequestTokenTemplate).Post<TokenResponse>(new { }).Body;
      var response = Session.Bind(TwitterApi.OAuthRequestTokenTemplate).Post<string>(new { }).Body;
      
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
