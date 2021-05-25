using System;
using System.Collections.Generic;
using System.Net.Cache;
using System.Text;
using Ramone.Utility.ObjectSerialization;
using System.Globalization;
using Ramone.Utility.Validation;

namespace Ramone.Implementation
{
  internal class RamoneService : IService
  {
    #region IRamoneService Members

    public string UserAgent { get; set; }

    public Uri BaseUri { get; protected set; }

    public RequestCachePolicy CachePolicy { get; set; }

    public Encoding DefaultEncoding { get; set; }

    public MediaType DefaultRequestMediaType { get; set; }

    public MediaType DefaultResponseMediaType { get; set; }

    public ICodecManager CodecManager { get; protected set; }

    public IAuthorizationDispatcher AuthorizationDispatcher { get; protected set; }

    public IRequestInterceptorSet RequestInterceptors { get; protected set; }

    public IResponseInterceptorSet ResponseInterceptors { get; protected set; }

    public ObjectSerializerSettings SerializerSettings { get; set; }

    public IDictionary<string, object> Items { get; protected set; }


    public ISession NewSession()
    {
      return new RamoneSession(this);
    }


    public void SetAllowedRedirects(int responseCode, int redirectCount)
    {
      AllowedRedirectsMap[responseCode] = redirectCount;
    }


    public int GetAllowedRedirects(int responseCode)
    {
      if (AllowedRedirectsMap.ContainsKey(responseCode))
        return AllowedRedirectsMap[responseCode];
      if (responseCode == 303)
        return 10;
      else
        return 0;
    }

    
    public void CopyRedirect(ISession session)
    {
      foreach (int code in AllowedRedirectsMap.Keys)
      {
        session.SetAllowedRedirects(code, AllowedRedirectsMap[code]);
      }
    }


    public IService AlwaysAccept(MediaType accept, double? q = null)
    {
      Condition.Requires(accept, "accept").IsNotNull();
      if (!string.IsNullOrEmpty(AlwaysAcceptHeader))
        AlwaysAcceptHeader += ", ";
      AlwaysAcceptHeader += (string)accept;
      if (q != null)
        AlwaysAcceptHeader += string.Format(CultureInfo.InvariantCulture, "; q={0:F2}", q);
      return this;
    }

    public string AlwaysAcceptHeader { get; set; }

    #endregion


    protected Dictionary<int, int> AllowedRedirectsMap { get; set; }


    public RamoneService(Uri baseUri)
    {
      BaseUri = baseUri;
      DefaultEncoding = Encoding.UTF8;
      CodecManager = new CodecManager();
      AuthorizationDispatcher = new AuthorizationDispatcher();
      RequestInterceptors = new RequestInterceptorSet();
      ResponseInterceptors = new ResponseInterceptorSet();
      SerializerSettings = new ObjectSerializerSettings();
      AllowedRedirectsMap = new Dictionary<int, int>();
      Items = new Dictionary<string, object>();
    }
  }
}
