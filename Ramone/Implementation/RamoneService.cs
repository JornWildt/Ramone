using System;
using System.Net;


namespace Ramone.Implementation
{
  public class RamoneService : IService
  {
    #region IRamoneService Members

    public ISettings Settings { get; protected set; }

    public string UserAgent { get; set; }

    public Uri BaseUri { get; protected set; }

    public IAuthorizationDispatcher AuthorizationDispatcher { get; protected set; }

    public IRequestInterceptorSet RequestInterceptors { get; protected set; }

    public ISession NewSession()
    {
      return new RamoneSession(this);
    }

    #endregion


    public RamoneService(ISettings settings, Uri baseUri)
    {
      Settings = settings;
      UserAgent = settings.UserAgent;
      BaseUri = baseUri;
      AuthorizationDispatcher = new AuthorizationDispatcher();
      RequestInterceptors = new RequestInterceptorSet();
    }
  }
}
