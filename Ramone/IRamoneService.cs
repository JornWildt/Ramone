using System;
using System.Net;
using Ramone.Utility.ObjectSerialization;


namespace Ramone
{
  public interface IRamoneService : IHaveRequestInterceptors
  {
    ICodecManager CodecManager { get; }

    string UserAgent { get; set; }

    Uri BaseUri { get; }

    MediaType DefaultRequestMediaType { get; set; }

    MediaType DefaultResponseMediaType { get; set; }

    IAuthorizationDispatcher AuthorizationDispatcher { get; }

    ObjectSerializerSettings SerializerSettings { get; set; }

    IRamoneSession NewSession();
  }
}
