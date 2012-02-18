using System;
using System.Net;
using System.Text;


namespace Ramone.AuthorizationInterceptors
{
  public class BasicAuthorizationInterceptor : IRequestInterceptor
  {
    string Username;
    string Passsword;


    public BasicAuthorizationInterceptor(string username, string password)
    {
      Username = username;
      Passsword = password;
    }


    public void Intercept(RequestContext context)
    {
      string token = Convert.ToBase64String(Encoding.ASCII.GetBytes(Username + ":" + Passsword));
      context.Request.Headers["Authorization"] = "Basic " + token;
    }
  }
}
