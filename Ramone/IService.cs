using System;
using System.Net;


namespace Ramone
{
  public interface IService
  {
    ISettings Settings { get; }

    string UserAgent { get; set; }

    Uri BaseUri { get; }

    IAuthorizationDispatcher AuthorizationDispatcher { get; }

    IRequestInterceptorSet RequestInterceptors { get; }

    ISession NewSession();
  }
}
