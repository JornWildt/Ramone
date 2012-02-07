using System;
using System.Net;
using Ramone.Utility.ObjectSerialization;


namespace Ramone
{
  public interface IRamoneSession : IHaveRequestInterceptors
  {
    IRamoneService Service { get; }

    CookieContainer Cookies { get; }

    string UserAgent { get; set; }

    Uri BaseUri { get; }

    string DefaultRequestMediaType { get; set; }

    string DefaultResponseMediaType { get; set; }
    
    IAuthorizationDispatcher AuthorizationDispatcher { get; }

    ObjectSerializerSettings SerializerSettings { get; set; }
  }
}
