using System;
using System.Text;
using Ramone.Utility.ObjectSerialization;


namespace Ramone.Implementation
{
  internal class RamoneService : IService
  {
    #region IRamoneService Members

    public string UserAgent { get; set; }

    public Uri BaseUri { get; protected set; }

    public Encoding DefaultEncoding { get; set; }

    public MediaType DefaultRequestMediaType { get; set; }

    public MediaType DefaultResponseMediaType { get; set; }

    public ICodecManager CodecManager { get; protected set; }

    public IAuthorizationDispatcher AuthorizationDispatcher { get; protected set; }

    public IRequestInterceptorSet RequestInterceptors { get; protected set; }

    public ObjectSerializerSettings SerializerSettings { get; set; }


    public ISession NewSession()
    {
      return new RamoneSession(this);
    }

    #endregion


    public RamoneService(Uri baseUri)
    {
      BaseUri = baseUri;
      DefaultEncoding = Encoding.UTF8;
      CodecManager = new CodecManager();
      AuthorizationDispatcher = new AuthorizationDispatcher();
      RequestInterceptors = new RequestInterceptorSet();
      SerializerSettings = new ObjectSerializerSettings();
    }
  }
}
