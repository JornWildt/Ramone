using System;
using System.Net;


namespace Ramone.Implementation
{
  internal class RamoneService : IRamoneService
  {
    #region IRamoneService Members

    public string UserAgent { get; set; }

    public Uri BaseUri { get; protected set; }

    public ICodecManager CodecManager { get; protected set; }

    public IAuthorizationDispatcher AuthorizationDispatcher { get; protected set; }

    public IRequestInterceptorSet RequestInterceptors { get; protected set; }

    public IRamoneSession NewSession()
    {
      return new RamoneSession(this);
    }

    #endregion


    public RamoneService(Uri baseUri)
    {
      UserAgent = "Ramone/1.0";
      BaseUri = baseUri;
      CodecManager = new CodecManager();
      AuthorizationDispatcher = new AuthorizationDispatcher();
      RequestInterceptors = new RequestInterceptorSet();
    }
  }
}
