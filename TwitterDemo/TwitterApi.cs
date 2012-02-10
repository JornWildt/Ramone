using System;


namespace TwitterDemo
{
  public class TwitterApi
  {
    // OAuth
    public static UriTemplate OAuthRequestTokenTemplate = new UriTemplate("oauth/request_token");

    public static UriTemplate UserTimeLineTemplate = new UriTemplate("/1/statuses/user_timeline.json?screen_name={screen_name}&count={count}");
    public static UriTemplate UpdateProfileTemplate = new UriTemplate("/1/account/update_profile.json?name={name}");
  }
}
