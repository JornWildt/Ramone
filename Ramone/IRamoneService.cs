using System;
using System.Net;
using Ramone.Utility.ObjectSerialization;


namespace Ramone
{
  public interface IRamoneService
  {
    ICodecManager CodecManager { get; }

    string UserAgent { get; set; }

    Uri BaseUri { get; }

    IAuthorizationDispatcher AuthorizationDispatcher { get; }

    IRequestInterceptorSet RequestInterceptors { get; }

    ObjectSerializerSettings SerializerSettings { get; set; }

    IRamoneSession NewSession();
  }
}
