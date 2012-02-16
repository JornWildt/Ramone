using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Ramone.Utility;


namespace Ramone
{
  public static class BindingExtensions
  {
    #region UriTemplate

    public static RamoneRequest Bind(this IRamoneSession session, UriTemplate template, object parameters)
    {
      return Bind(session, session.BaseUri, template, parameters);
    }


    private static RamoneRequest Bind(this IRamoneSession session, Uri baseUri, UriTemplate template, object parameters)
    {
      Uri url = BindTemplate(baseUri, template, parameters);
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

    #endregion


    #region Uri as template

    public static RamoneRequest Bind(this IRamoneSession session, Uri url, object parameters)
    {
      Uri baseUri = new Uri(url.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped));
      UriTemplate template = new UriTemplate(url.GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped));
      return Bind(session, baseUri, template, parameters);
    }

    #endregion


    private static Uri BindTemplate(Uri baseUri, UriTemplate template, object parameters)
    {
      if (parameters is IDictionary<string, string>)
      {
        return template.BindByName(baseUri, (IDictionary<string, string>)parameters);
      }
      else if (parameters is NameValueCollection)
      {
        return template.BindByName(baseUri, (NameValueCollection)parameters);
      }
      else
      {
        Dictionary<string, string> parameterDictionary = DictionaryConverter.ConvertObjectPropertiesToDictionary(parameters);
        return template.BindByName(baseUri, parameterDictionary);
      }
    }
  }
}
