using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Ramone.Utility;
using Ramone.HyperMedia;


namespace Ramone
{
  public static class BindingExtensions
  {
    #region UriTemplate

    public static Request Bind(this IRamoneSession session, UriTemplate template, object parameters = null)
    {
      Uri url = BindUri(session, template, parameters);
      return session.Request(url);
    }


    public static Uri BindUri(this IRamoneSession session, UriTemplate template, object parameters = null)
    {
      return BindTemplate(session.BaseUri, template, parameters);
    }

    #endregion


    #region String template

    public static Request Bind(this IRamoneSession session, string url, object parameters = null)
    {
      Uri boundUrl = BindUri(session, url, parameters);
      return session.Request(boundUrl);
    }


    public static Uri BindUri(this IRamoneSession session, string url, object parameters = null)
    {
      Uri absoluteUri;
      if (Uri.TryCreate(url, UriKind.Absolute, out absoluteUri))
      {
        // String as absolute URI template
        return BindUri(session, absoluteUri, parameters);
      }
      else
      {
        // String as relative path template
        UriTemplate template = new UriTemplate(url);
        return BindUri(session, template, parameters);
      }
    }

    #endregion


    #region Uri as template

    public static Request Bind(this IRamoneSession session, Uri url, object parameters = null)
    {
      Uri boundUrl = BindUri(session, url, parameters);
      return session.Request(boundUrl);
    }


    public static Uri BindUri(this IRamoneSession session, Uri url, object parameters = null)
    {
      Uri baseUri = new Uri(url.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped));
      UriTemplate template = new UriTemplate(url.GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped));

      return BindTemplate(baseUri, template, parameters);
    }

    #endregion


    #region IUrlTemplate

    public static Request Bind(this IRamoneSession session, ILinkTemplate link, object parameters = null)
    {
      Uri boundUrl = BindUri(session, link.Template, parameters);
      return session.Request(boundUrl);
    }

    #endregion


    private static Uri BindTemplate(Uri baseUri, UriTemplate template, object parameters = null)
    {
      if (parameters == null)
      {
        Dictionary<string, string> emptyParameters = new Dictionary<string, string>();
        return template.BindByName(baseUri, emptyParameters);
      }
      else if (parameters is IDictionary<string, string>)
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
