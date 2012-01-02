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


    public void Intercept(HttpWebRequest request)
    {
      string token = Convert.ToBase64String(Encoding.ASCII.GetBytes(Username + ":" + Passsword));
      request.Headers["Authorization"] = "Basic " + token;
    }
  }
}
