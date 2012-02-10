using System;
using System.IO;
using Ramone.Utility.ObjectSerialization;
using System.Collections.Specialized;
using System.Web;
using CuttingEdge.Conditions;


namespace Ramone.Utility
{
  public class FormUrlEncodingSerializer
  {
    protected ObjectSerializer Serializer;


    public FormUrlEncodingSerializer(Type t)
    {
      Serializer = new ObjectSerializer(t);
    }


    public void Serialize(TextWriter writer, object data, ObjectSerializerSettings settings = null)
    {
      Condition.Requires(writer, "writer").IsNotNull();

      FormUrlEncodingPropertyVisitor v = new FormUrlEncodingPropertyVisitor(writer);
      Serializer.Serialize(data, v, settings);
    }


    public object Deserialize(TextReader reader)
    {
      Condition.Requires(reader, "reader").IsNotNull();

      string data = reader.ReadToEnd();
      NameValueCollection values = HttpUtility.ParseQueryString(data);
      return Serializer.Deserialize(values);
    }
  }
}
