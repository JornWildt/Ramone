using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Web;
using CuttingEdge.Conditions;
using Ramone.Utility;

namespace Ramone
{
  public static class UriExtensions
  {
    /// <summary>
    /// Add query parameters to URL while keeping exiting parameters already specified in the URL.
    /// </summary>
    /// <remarks>This method respects repeated keys, such that adding "x=3&amp;x=4" to "x=1&amp;x=2" yields "x=1&amp;x=2&amp;x=3&amp;x=4".</remarks>
    /// <param name="url"></param>
    /// <param name="parameters">Either IDictionary&lt;string,string&gt;, NameValueCollection or any other
    /// class where the public properties are added as query parameters.</param>
    /// <returns>New Uri with added parameters</returns>
    public static Uri AddQueryParameters(this Uri url, object parameters)
    {
      if (parameters == null)
        return url;

      Condition.Requires(url, "url").IsNotNull();

      NameValueCollection paramColl = HttpUtility.ParseQueryString(url.Query);

      if (parameters is IDictionary<string, string>)
      {
        foreach (KeyValuePair<string, string> p in (IDictionary<string, string>)parameters)
          paramColl.Add(p.Key, p.Value);
      }
      else if (parameters is NameValueCollection)
      {
        NameValueCollection pcol = (NameValueCollection)parameters;
        paramColl.Add(pcol);
        //foreach (string key in pcol.AllKeys)
        //  paramColl.Add(key, pcol[key]);
      }
      else
      {
        Dictionary<string, string> parameterDictionary = DictionaryConverter.ConvertObjectPropertiesToDictionary(parameters);
        foreach (KeyValuePair<string, string> p in parameterDictionary)
          paramColl.Add(p.Key, p.Value);
      }

      string q = paramColl.ToString();

      return new Uri(url.GetLeftPart(UriPartial.Path) + "?" + q);
    }
  }
}
