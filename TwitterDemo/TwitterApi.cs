using System;


namespace TwitterDemo
{
  public class TwitterApi
  {
    // OAuth
    public static string OAuthRequestTokenPath = "oauth/request_token";
    public static string OAuthAuthorizePath = "oauth/authorize";
    public static string OAuthAccessTokenPath = "oauth/access_token";

    // Account
    public static UriTemplate UserTimeLineTemplate = new UriTemplate("/1/statuses/user_timeline.json?screen_name={screen_name}&count={count}");

    // Tweets and timeline
    public static UriTemplate UpdateProfileTemplate = new UriTemplate("/1/account/update_profile.json?name={name}");
  }
}
