using System.Net;


namespace Ramone
{
  public class AuthorizationContext
  {
    public ISession Session { get; protected set; }
    public HttpWebResponse Response { get; protected set; }
    public string Scheme { get; protected set; }
    public string Parameters { get; protected set; }

    public AuthorizationContext(ISession session, HttpWebResponse response, string scheme, string parameters)
    {
      Session = session;
      Response= response;
      Scheme = scheme;
      Parameters = parameters;
    }
  }


  public interface IAuthorizationHandler
  {
    bool HandleAuthorizationRequest(AuthorizationContext context);
  }
}
