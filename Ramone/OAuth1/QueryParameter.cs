using System.Collections.Generic;
using System.Text;
using CuttingEdge.Conditions;


/// <summary>
/// OAuth utility originally from http://code.google.com/p/oauth/
/// </summary>
namespace Ramone.OAuth1
{
  public class QueryParameter
  {
    static Encoding Encoder8bit = Encoding.GetEncoding("iso-8859-1");

    public string Name { get; private set; }
    public string Value { get; private set; }

    public QueryParameter(string name, string value)
    {
      Condition.Requires(name, "name").IsNotNull();
      
      if (value == null)
        value = "";

      Name = SignatureHelper.UrlEncode(Encoder8bit.GetString(Encoding.UTF8.GetBytes(name)));
      Value = SignatureHelper.UrlEncode(Encoder8bit.GetString(Encoding.UTF8.GetBytes(value)));
    }
  }


  /// <summary>
  /// Comparer class used to perform the sorting of the query parameters
  /// </summary>
  public class QueryParameterComparer : IComparer<QueryParameter>
  {
    #region IComparer<QueryParameter> Members

    public int Compare(QueryParameter x, QueryParameter y)
    {
      if (x.Name == y.Name)
      {
        return string.Compare(x.Value, y.Value);
      }
      else
      {
        return string.Compare(x.Name, y.Name);
      }
    }

    #endregion
  }
}
