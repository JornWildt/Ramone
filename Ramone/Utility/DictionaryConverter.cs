using System;
using System.Collections.Generic;
using System.Reflection;


namespace Ramone.Utility
{
  public static class DictionaryConverter
  {
    public static Dictionary<string, string> ConvertObjectPropertiesToDictionary(object src)
    {
      if (src is Dictionary<string, string>)
        return (Dictionary<string, string>)src;

      Dictionary<string, string> result = new Dictionary<string, string>();

      if (src == null)
        return result;

      Type t = src.GetType();
      ObjectSerializer Serializer = new ObjectSerializer(t);

      DictionaryConverterPropertyVisitor visitor = new DictionaryConverterPropertyVisitor();
      Serializer.Serialize(src, visitor);

      return visitor.Result;
    }
  }
}
