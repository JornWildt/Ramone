using System;
using System.Net;
using System.Text;


namespace Ramone.AuthorizationInterceptors
{
  public class BasicAuthorizationInterceptor : IRequestInterceptor
  {
    string Username;
    string Password;


    public BasicAuthorizationInterceptor(string username, string password)
    {
      Username = username;
      Password = password;
    }


    public void HeadersReady(RequestContext context)
    {
      string token = Convert.ToBase64String(Encoding.ASCII.GetBytes(Username + ":" + Password));
      context.Request.Headers["Authorization"] = "Basic " + token;
    }


    public void DataSent(RequestContext context)
    {
    }
  }
}
