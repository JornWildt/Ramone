using System;
using System.Collections.Generic;
using Ramone.Utility.ObjectSerialization;
using System.Collections;


namespace Ramone.Utility
{
  public static class HashtableConverter
  {
    public static Hashtable ConvertObjectPropertiesToHashtable(object src)
    {
      Hashtable result = new Hashtable();

      if (src == null)
        return result;

      Type t = src.GetType();
      ObjectSerializer Serializer = new ObjectSerializer(t);

      HashtableConverterPropertyVisitor visitor = new HashtableConverterPropertyVisitor();
      Serializer.Serialize(src, visitor);

      return visitor.Result;
    }
  }
}
