using System;
using System.Net;
using Ramone.Utility;


namespace Ramone
{
  public interface IRamoneSession
  {
    IRamoneService Service { get; }

    CookieContainer Cookies { get; }

    string UserAgent { get; set; }

    Uri BaseUri { get; }
    
    IAuthorizationDispatcher AuthorizationDispatcher { get; }

    IRequestInterceptorSet RequestInterceptors { get; }

    ObjectSerializerSettings FormUrlEncodedSerializerSettings { get; set; }

    ObjectSerializerSettings MultipartFormDataSerializerSettings { get; set; }
  }
}
