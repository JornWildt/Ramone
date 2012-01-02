using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Web;

namespace Ramone.Utility
{
  public class FormUrlEncodingSerializer
  {
    protected Type DataType { get; set; }


    public FormUrlEncodingSerializer(Type t)
    {
      DataType = t;
    }


    public void Serialize(TextWriter w, object data)
    {
      if (data == null)
        return;
      if (data.GetType() != DataType)
        throw new ArgumentException(string.Format("Cannot serialize {0} - expected {1}.", data.GetType(), DataType), "data");

      Serialize(w, data, DataType, "", true);
    }


    protected void Serialize(TextWriter w, object data, Type dataType, string prefix, bool firstValue)
    {
      if (data == null)
        return;

      foreach (PropertyInfo p in dataType.GetProperties())
      {
        string propertyName = p.Name;
        object propertyValue = p.GetValue(data, null);
        string propertyValueString = null;

        if (propertyValue != null)
        {
          propertyValueString = propertyValue.ToString();
        }

        if (propertyValueString != null)
        {
          if (!firstValue)
            w.Write("&");
          w.Write(HttpUtility.UrlEncode(propertyName) + "=" + HttpUtility.UrlEncode(propertyValueString));
          firstValue = false;
        }
      }
    }
  }
}
