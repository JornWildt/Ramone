using System;
using System.Net;
using Ramone.Utility.ObjectSerialization;


namespace Ramone.Implementation
{
  public class RamoneSession : IRamoneSession
  {
    #region IRamoneSession Members

    public IRamoneService Service { get; protected set; }

    public string UserAgent { get; set; }
    
    public Uri BaseUri { get; protected set; }

    public CookieContainer Cookies { get; protected set; }

    public IAuthorizationDispatcher AuthorizationDispatcher { get; protected set; }

    public IRequestInterceptorSet RequestInterceptors { get; protected set; }

    public ObjectSerializerSettings SerializerSettings { get; set; }

    #endregion


    public RamoneSession(IRamoneService service)
    {
      Service = service;
      UserAgent = service.UserAgent;
      BaseUri = Service.BaseUri;
      Cookies = new CookieContainer();
      AuthorizationDispatcher = service.AuthorizationDispatcher.Clone();
      RequestInterceptors = service.RequestInterceptors.Clone();
      SerializerSettings = new ObjectSerializerSettings(service.SerializerSettings);
    }
  }
}
