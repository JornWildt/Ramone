using System;
using System.IO;
using Ramone.Utility.ObjectSerialization;
using System.Collections.Specialized;
using System.Web;
using CuttingEdge.Conditions;
using System.Text;


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


    /// <summary>
    /// Deserialize form-urlencoded data from reader.
    /// </summary>
    /// <param name="reader">A TextReader for reading url-encoded string input.</param>
    /// <param name="charset">Name of character set used to decode international characters after the 
    /// form-urlencoded input has been decoded to a byte stream.</param>
    /// <returns></returns>
    public object Deserialize(TextReader reader, ObjectSerializerSettings settings = null)
    {
      Condition.Requires(reader, "reader").IsNotNull();

      string data = reader.ReadToEnd();
      Encoding enc = (settings != null ? settings.Charset ?? Encoding.UTF8 : Encoding.UTF8);
      NameValueCollection values = HttpUtility.ParseQueryString(data, enc);
      return Serializer.Deserialize(values);
    }
  }
}
