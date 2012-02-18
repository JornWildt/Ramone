using System;


namespace TwitterDemo
{
  public class TwitterApi
  {
    // OAuth
    public static string OAuthRequestTokenPath = "oauth/request_token";
    public static string OAuthAuthorizePath = "oauth/authorize";
    public static string OAuthAccessTokenPath = "oauth/access_token";

    // Tweets and timeline
    public static UriTemplate UserTimeLinePath = new UriTemplate("/1/statuses/user_timeline.json?screen_name={screen_name}&count={count}");
    public static UriTemplate StatusesUpdate = new UriTemplate("/1/statuses/update.json?status={status}");

    // Account
    public static UriTemplate UpdateProfilePath = new UriTemplate("/1/account/update_profile.json?name={name}");
  }
}
