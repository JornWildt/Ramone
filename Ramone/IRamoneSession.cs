using System;
using System.Net;


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
  }
}
