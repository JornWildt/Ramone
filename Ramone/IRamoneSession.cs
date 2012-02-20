using System;
using System.Net;
using System.Text;
using Ramone.Utility.ObjectSerialization;


namespace Ramone
{
  public interface IRamoneSession : IHaveRequestInterceptors
  {
    IRamoneService Service { get; }

    CookieContainer Cookies { get; }

    string UserAgent { get; set; }

    Uri BaseUri { get; }

    Encoding DefaultEncoding { get; set; }

    MediaType DefaultRequestMediaType { get; set; }

    MediaType DefaultResponseMediaType { get; set; }
    
    IAuthorizationDispatcher AuthorizationDispatcher { get; }

    ObjectSerializerSettings SerializerSettings { get; set; }
  }
}
