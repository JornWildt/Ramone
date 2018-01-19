using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Cache;
using System.Text;
using Ramone.Utility.ObjectSerialization;


namespace Ramone
{
  public interface ISession : IHaveRequestInterceptors, IHaveResponseInterceptors
  {
    IService Service { get; }

    CookieContainer Cookies { get; }

    string UserAgent { get; set; }

    Uri BaseUri { get; }

    RequestCachePolicy CachePolicy { get; set; }

    /// <summary>
    /// Assign one or more media types to be included in the Accept header on all request.
    /// </summary>
    /// <param name="accept"></param>
    /// <param name="q"></param>
    ISession AlwaysAccept(MediaType accept, double? q = null);

    string AlwaysAcceptHeader { get; }

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
