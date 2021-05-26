using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Ramone.Utility;
using Ramone.HyperMedia;
using System.Net;
using Ramone.Utility.Validation;
using Tavis.UriTemplates;

namespace Ramone
{
  public static class BindingExtensions
  {
    #region UriTemplate

    /// <summary>
    /// Resolve URI template, add session base URL and create a request bound to the session.
    /// </summary>
    /// <param name="session"></param>
    /// <param name="template"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static Request Bind(this ISession session, UriTemplate template, object parameters = null)
    {
      Uri url = BindUri(session, template, parameters);
      return session.Request(url);
    }


    /// <summary>
    /// Resolve URI template, add session base URL and create a URI.
    /// </summary>
    /// <param name="session"></param>
    /// <param name="template"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static Uri BindUri(this ISession session, UriTemplate template, object parameters = null)
    {
      return BindTemplate(session.BaseUri, template, parameters);
    }


    /// <summary>
    /// Resolve URI UriTemplate with supplied base URI and create request with implicit session.
    /// </summary>
    /// <param name="template"></param>
    /// <param name="baseUri"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static Request Bind(this UriTemplate template, Uri baseUri, object parameters = null)
    {
      Uri url = BindTemplate(baseUri, template, parameters);
      return new Request(url);
    }

    #endregion


    #region String template


    /// <summary>
    /// Resolve string URI UriTemplate with session base URL and create a request bound to the session.
    /// </summary>
    /// <param name="session"></param>
    /// <param name="url"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static Request Bind(this ISession session, string url, object parameters = null)
    {
      Uri boundUrl = BindUri(session, url, parameters);
      return session.Request(boundUrl);
    }


    /// <summary>
    /// Resolve string URI UriTemplate with session base URL and create a URI.
    /// </summary>
    /// <param name="session"></param>
    /// <param name="url"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static Uri BindUri(this ISession session, string url, object parameters = null)
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


    /// <summary>
    /// Resolve absolute string URI UriTemplate and create request with implicit session.
    /// </summary>
    /// <param name="url"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static Request Bind(this string url, object parameters = null)
    {
      Condition.Requires(url, "url").IsNotNull();

      Uri uri = new Uri(url);
      Uri baseUri = new Uri(uri.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped));
      UriTemplate template = new UriTemplate(uri.GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped));

      Uri boundUrl = BindTemplate(baseUri, template, parameters);
      return new Request(boundUrl);
    }

    #endregion


    #region Uri as template

    /// <summary>
    /// Bind URI UriTemplate with session base URI and create request bound to the session.
    /// </summary>
    /// <param name="session"></param>
    /// <param name="url"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static Request Bind(this ISession session, Uri url, object parameters = null)
    {
      Condition.Requires(url, "url").IsNotNull();

      Uri boundUrl = BindUri(session, url, parameters);
      return session.Request(boundUrl);
    }


    /// <summary>
    /// Bind URI UriTemplate with session base URI and create new URI.
    /// </summary>
    /// <param name="session"></param>
    /// <param name="url"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static Uri BindUri(this ISession session, Uri url, object parameters = null)
    {
      Condition.Requires(url, "url").IsNotNull();

      Uri baseUri;
      if (url.IsAbsoluteUri)
        baseUri = new Uri(url.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped));
      else
        baseUri = session.BaseUri;
      UriTemplate template = new UriTemplate(url.GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped));

      return BindTemplate(baseUri, template, parameters);
    }


    /// <summary>
    /// Resolve absolute URI UriTemplate and create request with implicit session.
    /// </summary>
    /// <param name="url"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static Request Bind(this Uri url, object parameters = null)
    {
      Condition.Requires(url, "url").IsNotNull();

      if (parameters is ISession)
        throw new ArgumentException("Do not use session as Bind() parameter. You probably should have written 'Session.Bind(url)'.", "parameters");

      Uri baseUri = new Uri(url.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped));
      UriTemplate template = new UriTemplate(url.GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped));

      Uri boundUrl = BindTemplate(baseUri, template, parameters);
      return new Request(boundUrl);
    }

    #endregion


    #region ILink

    public static Request Bind(this ISession session, ILink link)
    {
      return session.Bind(link.HRef);
    }

    #endregion


    #region ILinkTemplate

    /// <summary>
    /// Resolve link UriTemplate and create request bound to existing session.
    /// </summary>
    /// <param name="session"></param>
    /// <param name="link"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static Request Bind(this ISession session, ILinkTemplate link, object parameters = null)
    {
      Uri boundUrl = BindUri(session, link.Template, parameters);
      return session.Request(boundUrl);
    }


    /// <summary>
    /// Resolve absolute link UriTemplate and create request with implicit session.
    /// </summary>
    /// <param name="link"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static Request Bind(this ILinkTemplate link, object parameters = null)
    {
      return Bind(link.Template, parameters);
    }

    #endregion


    /// <summary>
    /// Resolve URI UriTemplate with base URI and different types of arguments.
    /// </summary>
    /// <param name="baseUri">Base URI for resolving relative URI templates.</param>
    /// <param name="template">The URI UriTemplate to resolve.</param>
    /// <param name="parameters">Parameters for resolving URI UriTemplate (can be IDictionary<string, string>, NameValueCollection or 
    /// any object where property names are used to match parameter names.</param>
    /// <returns></returns>
    public static Uri BindTemplate(Uri baseUri, UriTemplate template, object parameters = null)
    {
      if (baseUri == null)
        throw new InvalidOperationException("It is not possible to bind relative URL templates without a base URL. Make sure session and/or service has been created with a base URL.");
      Condition.Requires(template, "template").IsNotNull();

      if (parameters == null)
      {
        Dictionary<string, string> emptyParameters = new Dictionary<string, string>();
        return template.BindByName(baseUri, emptyParameters);
      }
      else if (parameters is IDictionary<string, string> dp)
      {
        return template.BindByName(baseUri, dp);
      }
      else if (parameters is NameValueCollection nvp)
      {
        IDictionary<string, string> dictParameters = nvp.Cast<string>().ToDictionary(p => p, p => nvp[p]);
        return template.BindByName(baseUri, dictParameters);
      }
      else
      {
        Dictionary<string, string> parameterDictionary = DictionaryConverter.ConvertObjectPropertiesToDictionary(parameters);
        return template.BindByName(baseUri, parameterDictionary);
      }
    }


    public static Response<T> Decode<T>(this ISession session, WebException ex)
      where T : class
    {
      if (ex.Response is HttpWebResponse)
      {
        HttpWebResponse response = (HttpWebResponse)ex.Response;
        return new Response<T>(response, session, 0);
      }
      else
      {
        return null;
      }
    }


    public static Uri BindByName(this UriTemplate template, Uri baseUri, IDictionary<string, string> parameters)
    {
      //// Reset before creating
      //foreach (var parameter in template.GetParameterNames())
      //  template.ClearParameter(parameter);

      foreach (var item in parameters)
        template = template.AddParameter(item.Key, item.Value);

      return new Uri(baseUri, template.Resolve());
    }
  }
}
