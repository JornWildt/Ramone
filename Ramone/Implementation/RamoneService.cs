using System;
using System.Net;
using Ramone.Utility.ObjectSerialization;


namespace Ramone.Implementation
{
  internal class RamoneService : IRamoneService
  {
    #region IRamoneService Members

    public string UserAgent { get; set; }

    public Uri BaseUri { get; protected set; }

    public string DefaultRequestMediaType { get; set; }

    public string DefaultResponseMediaType { get; set; }

    public ICodecManager CodecManager { get; protected set; }

    public IAuthorizationDispatcher AuthorizationDispatcher { get; protected set; }

    public IRequestInterceptorSet RequestInterceptors { get; protected set; }

    public ObjectSerializerSettings SerializerSettings { get; set; }


    public IRamoneSession NewSession()
    {
      return new RamoneSession(this);
    }

    #endregion


    public RamoneService(Uri baseUri)
    {
      BaseUri = baseUri;
      CodecManager = new CodecManager();
      AuthorizationDispatcher = new AuthorizationDispatcher();
      RequestInterceptors = new RequestInterceptorSet();
      SerializerSettings = new ObjectSerializerSettings();
    }
  }
}
