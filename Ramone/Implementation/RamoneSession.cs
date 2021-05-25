﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Cache;
using System.Text;
using Ramone.Utility.ObjectSerialization;
using Ramone.Utility.Validation;

namespace Ramone.Implementation
{
  public class RamoneSession : ISession
  {
    #region IRamoneSession Members

    public IService Service { get; protected set; }

    public string UserAgent { get; set; }
    
    public Uri BaseUri { get; protected set; }

    public RequestCachePolicy CachePolicy { get; set; }

    public Encoding DefaultEncoding { get; set; }

    public MediaType DefaultRequestMediaType { get; set; }

    public MediaType DefaultResponseMediaType { get; set; }

    public CookieContainer Cookies { get; protected set; }

    public IAuthorizationDispatcher AuthorizationDispatcher { get; protected set; }

    public IRequestInterceptorSet RequestInterceptors { get; protected set; }

    public IResponseInterceptorSet ResponseInterceptors { get; protected set; }

    public ObjectSerializerSettings SerializerSettings { get; set; }

    public IDictionary<string, object> Items { get; protected set; }


    public void SetAllowedRedirects(int responseCode, int redirectCount)
    {
      AllowedRedirectsMap[responseCode] = redirectCount;
    }


    public int GetAllowedRedirects(int responseCode)
    {
      if (AllowedRedirectsMap.ContainsKey(responseCode))
        return AllowedRedirectsMap[responseCode];
      if (responseCode == 301 || responseCode == 302 || responseCode == 303 || responseCode == 307)
        return 10;
      else
        return 0;
    }


    public ISession AlwaysAccept(MediaType accept, double? q = null)
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


    public RamoneSession(IService service)
    {
      Service = service;
      UserAgent = service.UserAgent;
      BaseUri = Service.BaseUri;
      CachePolicy = Service.CachePolicy;
      AlwaysAcceptHeader = service.AlwaysAcceptHeader;
      DefaultEncoding = service.DefaultEncoding;
      DefaultRequestMediaType = service.DefaultRequestMediaType;
      DefaultResponseMediaType = service.DefaultResponseMediaType;
      Cookies = new CookieContainer();
      AuthorizationDispatcher = service.AuthorizationDispatcher.Clone();
      RequestInterceptors = (IRequestInterceptorSet)service.RequestInterceptors.Clone();
      ResponseInterceptors = (IResponseInterceptorSet)service.ResponseInterceptors.Clone();
      SerializerSettings = new ObjectSerializerSettings(service.SerializerSettings);
      AllowedRedirectsMap = new Dictionary<int, int>();
      service.CopyRedirect(this);
      Items = new Dictionary<string, object>(service.Items);
    }
  }
}
