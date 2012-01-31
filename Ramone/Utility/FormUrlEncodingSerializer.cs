using System;
using System.IO;
using System.Web;


namespace Ramone.Utility
{
  public class FormUrlEncodingSerializer
  {
    protected ObjectSerializer Serializer;


    public FormUrlEncodingSerializer(Type t)
    {
      Serializer = new ObjectSerializer(t);
    }


    public void Serialize(TextWriter w, object data, ObjectSerializerSettings settings = null)
    {
      FormUrlEncodingPropertyVisitor v = new FormUrlEncodingPropertyVisitor(w);
      Serializer.Serialize(data, v, settings);
    }
  }
}
