using Ramone.OAuth1;


namespace TwitterDemo
{
  // A small debugging class that allows you to set a break point and inspect OAuth data
  internal class OAuth1Logger : IOAuth1Logger
  {
    public void Log(OAuth1Settings settings, string message)
    {
    }
  }
}
