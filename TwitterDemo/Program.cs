using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ramone;

namespace TwitterDemo
{
  public class Program
  {
    static IRamoneSession Session { get; set; }


    static void Main(string[] args)
    {
      Setup();
      ShowStatusesForScreenName("JornWildt");
    }


    static void Setup()
    {
      Session = RamoneConfiguration.NewSession(new Uri("https://api.twitter.com/1/"));
    }


    static void ShowStatusesForScreenName(string screenName)
    {
      RamoneRequest request = Session.Bind(TwitterApi.UserTimeLineTemplate, new { screen_name = "JornWildt", count = 2 });

      RamoneResponse response = request.Get();

      dynamic timeline = response.Body;

      foreach (dynamic tweet in timeline)
      {
        Console.WriteLine("{0}.", tweet.text);
      }
    }
  }
}
