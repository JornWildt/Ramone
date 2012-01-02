using System;
using System.Net;


namespace Ramone
{
  public interface IRamoneService
  {
    IRamoneSettings Settings { get; }

    string UserAgent { get; set; }

    Uri BaseUri { get; }

    IAuthorizationDispatcher AuthorizationDispatcher { get; }

    IRequestInterceptorSet RequestInterceptors { get; }

    IRamoneSession NewSession();
  }
}
