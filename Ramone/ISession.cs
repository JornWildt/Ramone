using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Cache;
using System.Text;
using Ramone.Utility.ObjectSerialization;


namespace Ramone
{
  public interface ISession : IHaveRequestInterceptors
  {
    IService Service { get; }

    CookieContainer Cookies { get; }

    string UserAgent { get; set; }

    Uri BaseUri { get; }

    RequestCachePolicy CachePolicy { get; set; }

    Encoding DefaultEncoding { get; set; }

    MediaType DefaultRequestMediaType { get; set; }

    MediaType DefaultResponseMediaType { get; set; }
    
    IAuthorizationDispatcher AuthorizationDispatcher { get; }

    ObjectSerializerSettings SerializerSettings { get; set; }

    void SetAllowedRedirects(int responseCode, int redirectCount);

    int GetAllowedRedirects(int responseCode);

    IDictionary<string, object> Items { get; }
  }
}
