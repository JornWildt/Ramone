using System;
using System.Net;


namespace Ramone.Implementation
{
  public class RamoneSession : ISession
  {
    #region IRamoneSession Members

    public IService Service { get; protected set; }

    public string UserAgent { get; set; }
    
    public Uri BaseUri { get; protected set; }

    public CookieContainer Cookies { get; protected set; }

    public IAuthorizationDispatcher AuthorizationDispatcher { get; protected set; }

    public IRequestInterceptorSet RequestInterceptors { get; protected set; }

    #endregion


    public RamoneSession(IService service)
    {
      Service = service;
      UserAgent = service.UserAgent;
      BaseUri = Service.BaseUri;
      Cookies = new CookieContainer();
      AuthorizationDispatcher = service.AuthorizationDispatcher.Clone();
      RequestInterceptors = service.RequestInterceptors.Clone();
    }
  }
}
