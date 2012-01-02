using System;
using System.Collections.Generic;
using System.Reflection;


namespace Ramone.Utility
{
  public static class DictionaryConverter
  {
    public static Dictionary<string, string> ConvertObjectPropertiesToDictionary(object src)
    {
      Dictionary<string, string> result = new Dictionary<string, string>();

      if (src == null)
        return result;

      Type t = src.GetType();
      foreach (PropertyInfo fi in t.GetProperties())
      {
        string key = fi.Name;
        object value = ReadValue(fi, src);
        result[key] = value != null ? value.ToString() : "";
      }

      return result;
    }


    private static object ReadValue(PropertyInfo property, object data)
    {
      if (property.CanRead)
        return property.GetValue(data, null);
      throw new ArgumentException(string.Format("The value {0} cannot be read from {1}.", property.Name, property.DeclaringType));
    }
  }
}
