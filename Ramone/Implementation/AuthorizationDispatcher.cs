using System.Collections.Generic;


namespace Ramone.Implementation
{
  public class AuthorizationDispatcher : IAuthorizationDispatcher
  {
    private Dictionary<string, IAuthorizationHandler> Handlers;

    
    public AuthorizationDispatcher()
    {
      Handlers = new Dictionary<string, IAuthorizationHandler>();
    }


    public AuthorizationDispatcher(AuthorizationDispatcher src)
    {
      Handlers = new Dictionary<string, IAuthorizationHandler>(src.Handlers);
    }


    #region IAuthorizationDispatcher Members

    public void Add(string authorizationScheme, IAuthorizationHandler handler)
    {
      authorizationScheme = authorizationScheme.ToLower();
      Handlers.Add(authorizationScheme, handler);
    }


    public void Remove(string authorizationScheme)
    {
      Handlers.Remove(authorizationScheme);
    }


    public IAuthorizationHandler Get(string authorizationScheme)
    {
      authorizationScheme = authorizationScheme.ToLower();
      if (Handlers.ContainsKey(authorizationScheme))
        return Handlers[authorizationScheme];
      else
        return null;
    }


    public IAuthorizationDispatcher Clone()
    {
      return new AuthorizationDispatcher(this);
    }

    #endregion
  }
}
