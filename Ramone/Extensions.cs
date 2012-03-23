using System;
using System.Collections.Generic;
using System.Net;
using Ramone.Utility;


namespace Ramone
{
  public static class Extensions
  {
    public static Request Request(this ISession session, Uri url)
    {
      return new Request(session, url);
    }


    public static Request Request(this ISession session, string url)
    {
      return new Request(session, url);
    }


    public static Response<T> AsRamoneResponse<T>(this HttpWebResponse response, ISession session) where T : class
    {
      return new Response<T>(response, session);
    }


    public static Uri LocationAsUri(this HttpWebResponse response)
    {
      string location = response.Headers["Location"];
      if (location == null)
        return null;
      Uri url = new Uri(new Uri(response.ResponseUri.GetLeftPart(UriPartial.Authority)), location);
      return url;
    }


    public static Request AsXml(this Request request)
    {
      return request.ContentType(MediaType.ApplicationXml);
    }


    public static Request AcceptXml(this Request request)
    {
      return request.Accept(MediaType.ApplicationXml);
    }


    public static Request AsJson(this Request request)
    {
      return request.ContentType(MediaType.ApplicationJson);
    }


    public static Request AcceptJson(this Request request)
    {
      return request.Accept(MediaType.ApplicationJson);
    }


    public static Request AsFormUrlEncoded(this Request request)
    {
      return request.ContentType(MediaType.ApplicationFormUrlEncoded);
    }


    public static Request AsMultipartFormData(this Request request)
    {
      return request.ContentType("multipart/form-data");
    }
  }
}
