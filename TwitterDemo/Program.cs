using System;
using Ramone;
using System.Net;
using Ramone.Utility;


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
      Session = RamoneConfiguration.NewSession(new Uri("https://api.twitter.com/1/"));

      // Set default request/response media-type to JSON for Twitter.
      // This saves us the hassle of specifying codecs for all the Twitter resource types (Tweet, Timeline, User etc.)
      Session.DefaultRequestMediaType = "application/x-www-form-urlencoded";
      Session.DefaultResponseMediaType = "application/json";
      
      Session.RequestInterceptors.Add(new OAuthInterceptor());
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
      RamoneRequest request = Session.Bind(TwitterApi.UpdateProfileTemplate, new { name = "Peter Pedal 2" });
      RamoneResponse response = request.Post(new { });
    }


    static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();

    static double GetUnixTime()
    {
      TimeSpan span = (DateTime.Now.ToLocalTime() - epoch);
      return span.TotalSeconds;
    }


    public class OAuthInterceptor : IRequestInterceptor
    {
      #region IRequestInterceptor Members

      public void Intercept(HttpWebRequest request)
      {
        OAuthBase o = new OAuthBase();

        string consumer_key = "FrxT7YcvlC5l8H4cNvxp1A";
        string consumer_secret = "rL7ddb1g4OegHTLAInQAZGrF92RW72j6kQ9e1OXR6xI";

        string access_token = "348919657-4q4XSvhsuZuasUVTNpdmDWjCZzmDsP0yTMV819zh";
        string access_token_secret = "PAoxLvWhHVXZeOOUAHklKopZzblLZDXaHxv3DQd7I";

        string timestamp = o.GenerateTimeStamp();
        string nonce = o.GenerateNonce();

        //timestamp = "1328824349";
        //nonce = "c54be2858d15ba0e61f27353729e13d6";

        string url;
        string requestParams;

        string signature = o.GenerateSignature(request.RequestUri,
                                               consumer_key,
                                               consumer_secret,
                                               access_token,
                                               access_token_secret,
                                               request.Method,
                                               timestamp,
                                               nonce,
                                               OAuthBase.SignatureTypes.HMACSHA1,
                                               out url,
                                               out requestParams);

        //string signatureBaseString = o.GenerateSignatureBase(request.RequestUri,
        //                                                     consumer_key,
        //                                                     access_token,
        //                                                     access_token_secret,
        //                                                     request.Method.ToUpper(),
        //                                                     timestamp,
        //                                                     nonce,
        //                                                     "HMAC-SHA1",
        //                                                     out url,
        //                                                     out requestParams);

        string auth = string.Format(@"OAuth oauth_consumer_key=""{1}"", oauth_nonce=""{3}"", oauth_signature=""{6}"", oauth_signature_method=""{5}"", oauth_timestamp=""{4}"", oauth_token=""{2}"", oauth_version=""1.0""",

        url,
        consumer_key,
        access_token,
        nonce,
        timestamp,
        "HMAC-SHA1",
        o.UrlEncode(signature));

        request.Headers["Authorization"] = auth;
      }

      #endregion
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
