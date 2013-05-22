using System;
using System.Collections.Generic;
using System.Text;
using Ramone.Utility.ObjectSerialization;
using System.Net.Cache;


namespace Ramone
{
  public interface IService : IHaveRequestInterceptors
  {
    ICodecManager CodecManager { get; }

    string UserAgent { get; set; }

    Uri BaseUri { get; }

    RequestCachePolicy CachePolicy { get; set; }

    Encoding DefaultEncoding { get; set; }

    MediaType DefaultRequestMediaType { get; set; }

    MediaType DefaultResponseMediaType { get; set; }

    IAuthorizationDispatcher AuthorizationDispatcher { get; }

    ObjectSerializerSettings SerializerSettings { get; set; }

    void SetAllowedRedirects(int responseCode, int redirectCount);

    int GetAllowedRedirects(int responseCode);

    void CopyRedirect(ISession session);

    ISession NewSession();

    IDictionary<string, object> Items { get; }
  }
}
