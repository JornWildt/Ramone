using System;
using System.Text;


namespace Ramone.Utility
{
  public static class Base64Utility
  {
    public static string UTF8UrlEncode(string s)
    {
      byte[] bytes = Encoding.UTF8.GetBytes(s);
      return UrlEncode(bytes);
    }
    
    
    public static string UrlEncode(byte[] bytes)
    {
      return Convert.ToBase64String(bytes).Replace("=", String.Empty).Replace('+', '-').Replace('/', '_');
    }
  }
}
