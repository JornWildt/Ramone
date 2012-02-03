using Ramone.AuthorizationInterceptors;


namespace Ramone
{
  public static class AuthorizationExtensions
  {
    public static void BasicAuthentication(this IHaveRequestInterceptors interceptorOwner, string username, string password)
    {
      interceptorOwner.RequestInterceptors.Add(new BasicAuthorizationInterceptor(username, password));
    }
  }
}
