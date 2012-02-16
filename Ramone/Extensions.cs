using System;
using System.Collections.Generic;
using System.Net;
using Ramone.Utility;


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


    public static RamoneRequest AsXml(this RamoneRequest request)
    {
      return request.ContentType(MediaType.ApplicationXml).Accept(MediaType.ApplicationXml);
    }


    public static RamoneRequest AsJson(this RamoneRequest request)
    {
      return request.ContentType(MediaType.ApplicationJson).Accept(MediaType.ApplicationJson);
    }


    public static RamoneRequest AsFormUrlEncoded(this RamoneRequest request)
    {
      return request.ContentType(MediaType.ApplicationFormUrlEncoded);
    }


    public static RamoneRequest AsMultipartFormData(this RamoneRequest request)
    {
      return request.ContentType("multipart/form-data");
    }
  }
}
