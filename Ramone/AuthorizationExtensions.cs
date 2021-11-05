using System;
using System.Text;
using Ramone.AuthorizationInterceptors;


namespace Ramone
{
  public static class AuthorizationExtensions
  {
    public static void BasicAuthentication(this IHaveRequestInterceptors interceptorOwner, string username, string password)
    {
      interceptorOwner.RequestInterceptors.Remove(typeof(BasicAuthorizationInterceptor));
      interceptorOwner.RequestInterceptors.Add(new BasicAuthorizationInterceptor(username, password));
    }


    public static Request BasicAuthentication(this Request request, string username, string password)
    {
      string token = Convert.ToBase64String(Encoding.GetEncoding(28591).GetBytes(username + ":" + password));
      request.Header("Authorization", "BASIC " + token);
      return request;
    }
  }
}
