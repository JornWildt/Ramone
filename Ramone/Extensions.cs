using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Ramone.MediaTypes.Atom;
using Ramone.Utility;


namespace Ramone
{
  public static class Extensions
  {
    public static RamoneRequest Request(this ISession session, Uri url)
    {
      return new RamoneRequest(session, url);
    }


    public static RamoneRequest Request(this ISession session, string url)
    {
      return new RamoneRequest(session, url);
    }


    public static RamoneRequest Request(this ISession session, AtomLink link)
    {
      return new RamoneRequest(session, link.HRef);
    }


    public static RamoneResponse<T> AsRamoneResponse<T>(this HttpWebResponse response, ISession session) where T : class
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


    public static RamoneRequest Bind(this ISession session, UriTemplate template, object parameters)
    {
      Dictionary<string, string> parameterDictionary = DictionaryConverter.ConvertObjectPropertiesToDictionary(parameters);
      Uri url = template.BindByName(session.BaseUri, parameterDictionary);
      return session.Request(url);
    }


    public static RamoneRequest Bind(this ISession session, string url, object parameters = null)
    {
      UriTemplate template = new UriTemplate(url);
      return session.Bind(template, parameters);
    }


    public static AtomLink Link(this IHaveLinks links, string rel)
    {
      return links.Links.Where(l => l.RelationshipType == rel).FirstOrDefault();
    }


    public static RamoneRequest Follow<TResponse>(this RamoneResponse<TResponse> response, string rel)
      where TResponse : class, IHaveLinks
    {
      AtomLink link = response.Body.Link(rel);
      if (link == null)
        return null;

      return response.Session.Request(link);
    }
  }
}
