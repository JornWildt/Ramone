using System;


namespace Ramone.Utility
{
  public static class DateTimeUtility
  {
    public static long ToUnixTime(this DateTime now)
    {
      TimeSpan timeSpan = now - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
      double unixTime = timeSpan.TotalSeconds;
      return (long)unixTime;
    }
  }
}
