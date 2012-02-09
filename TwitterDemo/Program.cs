using System;
using Ramone;


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
      ShowTimelineForScreenName_Dynamic(TwitterUserScreenName);
      Console.WriteLine();
      ShowTimelineForScreenName_Static(TwitterUserScreenName);
    }


    static void Setup()
    {
      // Create new session with implicit service
      Session = RamoneConfiguration.NewSession(new Uri("https://api.twitter.com/1/"));

      // Set default request/response media-type to JSON for Twitter.
      // This saves us the hassle of specifying codecs for all the Twitter resource types (Tweet, Timeline, User etc.)
      Session.DefaultRequestMediaType = "application/json";
      Session.DefaultResponseMediaType = "application/json";
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
