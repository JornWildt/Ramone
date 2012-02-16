using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using Ramone.Utility;


namespace Ramone
{
  public static class BindingExtensions
  {
    #region UriTemplate

    public static RamoneRequest Bind(this IRamoneSession session, UriTemplate template, object parameters)
    {
      Dictionary<string, string> parameterDictionary = DictionaryConverter.ConvertObjectPropertiesToDictionary(parameters);
      return Bind(session, template, parameterDictionary);
    }


    public static RamoneRequest Bind(this IRamoneSession session, UriTemplate template, IDictionary<string, string> parameters)
    {
      Uri url = template.BindByName(session.BaseUri, parameters);
      return session.Request(url);
    }


    public static RamoneRequest Bind(this IRamoneSession session, UriTemplate template, NameValueCollection parameters)
    {
      Uri url = template.BindByName(session.BaseUri, parameters);
      return session.Request(url);
    }


    private static RamoneRequest Bind(this IRamoneSession session, Uri baseUri, UriTemplate template, object parameters)
    {
      Dictionary<string, string> parameterDictionary = DictionaryConverter.ConvertObjectPropertiesToDictionary(parameters);
      Uri url = template.BindByName(baseUri, parameterDictionary);
      return session.Request(url);
    }


    private static RamoneRequest Bind(this IRamoneSession session, Uri baseUri, UriTemplate template, IDictionary<string, string> parameters)
    {
      Uri url = template.BindByName(baseUri, parameters);
      return session.Request(url);
    }


    private static RamoneRequest Bind(this IRamoneSession session, Uri baseUri, UriTemplate template, NameValueCollection parameters)
    {
      Uri url = template.BindByName(baseUri, parameters);
      return session.Request(url);
    }

    #endregion


    #region String template

    public static RamoneRequest Bind(this IRamoneSession session, string url, object parameters)
    {
      Uri absoluteUri;
      if (Uri.TryCreate(url, UriKind.Absolute, out absoluteUri))
      {
        return Bind(session, absoluteUri, parameters);
      }
      else
      {
        UriTemplate template = new UriTemplate(url);
        return session.Bind(template, parameters);
      }
    }


    public static RamoneRequest Bind(this IRamoneSession session, string url, IDictionary<string, string> parameters)
    {
      Uri absoluteUri;
      if (Uri.TryCreate(url, UriKind.Absolute, out absoluteUri))
      {
        return Bind(session, absoluteUri, parameters);
      }
      else
      {
        UriTemplate template = new UriTemplate(url);
        return session.Bind(template, parameters);
      }
    }


    public static RamoneRequest Bind(this IRamoneSession session, string url, NameValueCollection parameters)
    {
      Uri absoluteUri;
      if (Uri.TryCreate(url, UriKind.Absolute, out absoluteUri))
      {
        return Bind(session, absoluteUri, parameters);
      }
      else
      {
        UriTemplate template = new UriTemplate(url);
        return session.Bind(template, parameters);
      }
    }

    #endregion


    #region Uri as template

    public static RamoneRequest Bind(this IRamoneSession session, Uri url, object parameters)
    {
      Uri baseUri = new Uri(url.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped));
      UriTemplate template = new UriTemplate(url.GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped));
      return Bind(session, baseUri, template, parameters);
    }


    public static RamoneRequest Bind(this IRamoneSession session, Uri url, IDictionary<string, string> parameters)
    {
      Uri baseUri = new Uri(url.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped));
      UriTemplate template = new UriTemplate(url.GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped));
      return Bind(session, baseUri, template, parameters);
    }
    
    
    public static RamoneRequest Bind(this IRamoneSession session, Uri url, NameValueCollection parameters)
    {
      Uri baseUri = new Uri(url.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped));
      UriTemplate template = new UriTemplate(url.GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped));
      return Bind(session, baseUri, template, parameters);
    }

    #endregion
  }
}
