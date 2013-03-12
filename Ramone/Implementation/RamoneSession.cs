using System;
using System.Net;
using System.Text;
using Ramone.Utility.ObjectSerialization;
using System.Collections.Generic;


namespace Ramone.Implementation
{
  public class RamoneSession : ISession
  {
    #region IRamoneSession Members

    public IService Service { get; protected set; }

    public string UserAgent { get; set; }
    
    public Uri BaseUri { get; protected set; }

    public Encoding DefaultEncoding { get; set; }

    public MediaType DefaultRequestMediaType { get; set; }

    public MediaType DefaultResponseMediaType { get; set; }

    public CookieContainer Cookies { get; protected set; }

    public IAuthorizationDispatcher AuthorizationDispatcher { get; protected set; }

    public IRequestInterceptorSet RequestInterceptors { get; protected set; }

    public ObjectSerializerSettings SerializerSettings { get; set; }

    public IDictionary<string, object> Items { get; protected set; }


    public void SetAllowedRedirects(int responseCode, int redirectCount)
    {
      AllowedRedirectsMap[responseCode] = redirectCount;
    }


    public int GetAllowedRedirects(int responseCode)
    {
      if (AllowedRedirectsMap.ContainsKey(responseCode))
        return AllowedRedirectsMap[responseCode];
      if (responseCode == 301 || responseCode == 302 || responseCode == 303 || responseCode == 307)
        return 10;
      else
        return 0;
    }

    #endregion


    protected Dictionary<int, int> AllowedRedirectsMap { get; set; }


    public RamoneSession(IService service)
    {
      Service = service;
      UserAgent = service.UserAgent;
      BaseUri = Service.BaseUri;
      DefaultEncoding = service.DefaultEncoding;
      DefaultRequestMediaType = service.DefaultRequestMediaType;
      DefaultResponseMediaType = service.DefaultResponseMediaType;
      Cookies = new CookieContainer();
      AuthorizationDispatcher = service.AuthorizationDispatcher.Clone();
      RequestInterceptors = service.RequestInterceptors.Clone();
      SerializerSettings = new ObjectSerializerSettings(service.SerializerSettings);
      AllowedRedirectsMap = new Dictionary<int, int>();
      service.CopyRedirect(this);
      Items = new Dictionary<string, object>(service.Items);
    }
  }
}
