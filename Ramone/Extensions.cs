using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Ramone.MediaTypes.Atom;
using Ramone.MediaTypes.FormUrlEncoded;
using Ramone.MediaTypes.Json;
using Ramone.MediaTypes.MultipartFormData;
using Ramone.MediaTypes.Xml;
using Ramone.Utility;
using System.Collections.Specialized;
using System.Web;


namespace Ramone
{
  public static class Extensions
  {
    public static RamoneRequest Request(this IRamoneSession session, Uri url)
    {
      return new RamoneRequest(session, url);
    }


    public static RamoneRequest Request(this IRamoneSession session, string url)
    {
      return new RamoneRequest(session, url);
    }


    public static RamoneResponse<T> AsRamoneResponse<T>(this HttpWebResponse response, IRamoneSession session) where T : class
    {
      return new RamoneResponse<T>(response, session);
    }


    public static Uri LocationAsUri(this HttpWebResponse response)
    {
      string location = response.Headers["Location"];
      if (location == null)
        return null;
      Uri url = new Uri(new Uri(response.ResponseUri.GetLeftPart(UriPartial.Authority)), location);
      return url;
    }


    public static Uri Bind(this UriTemplate template, Uri baseUrl, object parameters)
    {
      Dictionary<string, string> parameterDictionary = DictionaryConverter.ConvertObjectPropertiesToDictionary(parameters);
      return template.BindByName(baseUrl, parameterDictionary);
    }


    public static RamoneRequest Bind(this IRamoneSession session, UriTemplate template, object parameters = null)
    {
      Dictionary<string, string> parameterDictionary = DictionaryConverter.ConvertObjectPropertiesToDictionary(parameters);
      Uri url = template.BindByName(session.BaseUri, parameterDictionary);
      return session.Request(url);
    }


    public static RamoneRequest Bind(this IRamoneSession session, string url, object parameters = null)
    {
      UriTemplate template = new UriTemplate(url);
      return session.Bind(template, parameters);
    }


    public static RamoneRequest Bind(this IRamoneSession session, Uri url, object parameters = null)
    {
      NameValueCollection query = HttpUtility.ParseQueryString(url.Query);
      Dictionary<string, string> parameterDictionary = DictionaryConverter.ConvertObjectPropertiesToDictionary(parameters);
      foreach (KeyValuePair<string, string> p in parameterDictionary)
      {
        query.Add(p.Key, p.Value);
      }
      if (url.AbsoluteUri.Contains("?"))
        return new RamoneRequest(session, new Uri(url.AbsoluteUri + "&" + query.ToString()));
      else
        return new RamoneRequest(session, new Uri(url.AbsoluteUri + "?" + query.ToString()));
    }


    public static RamoneRequest AsXml(this RamoneRequest request)
    {
      return request.ContentType("application/xml").Accept("application/xml");
    }


    public static RamoneRequest AsJson(this RamoneRequest request)
    {
      return request.ContentType("application/json").Accept("application/json");
    }


    public static RamoneRequest AsFormUrlEncoded(this RamoneRequest request)
    {
      return request.ContentType("application/x-www-form-urlencoded");
    }


    public static RamoneRequest AsMultipartFormData(this RamoneRequest request)
    {
      return request.ContentType("multipart/form-data");
    }


    public static void AddFormUrlEncoded<T>(this ICodecManager codecManager)
    {
      codecManager.AddCodec<T>("application/x-www-form-urlencoded", new FormUrlEncodedSerializerCodec());
    }


    public static void AddFormUrlEncoded<T>(this ICodecManager codecManager, string mediaType)
    {
      codecManager.AddCodec<T>(mediaType, new FormUrlEncodedSerializerCodec());
    }


    public static void AddMultipartFormData<T>(this ICodecManager codecManager)
    {
      codecManager.AddCodec<T>("multipart/form-data", new MultipartFormDataSerializerCodec());
    }


    public static void AddMultipartFormData<T>(this ICodecManager codecManager, string mediaType)
    {
      codecManager.AddCodec<T>(mediaType, new MultipartFormDataSerializerCodec());
    }


    public static void AddXml<T>(this ICodecManager codecManager)
    {
      codecManager.AddCodec<T>("application/xml", new XmlSerializerCodec());
    }


    public static void AddXml<T>(this ICodecManager codecManager, string mediaType)
    {
      codecManager.AddCodec<T>(mediaType, new XmlSerializerCodec());
    }


    public static void AddJson<T>(this ICodecManager codecManager)
    {
      codecManager.AddCodec<T>("application/json", new JsonSerializerCodec());
    }


    public static void AddJson<T>(this ICodecManager codecManager, string mediaType)
    {
      codecManager.AddCodec<T>(mediaType, new JsonSerializerCodec());
    }
  }
}
