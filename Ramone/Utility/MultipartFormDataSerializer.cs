using System;
using System.IO;
using System.Web;


namespace Ramone.Utility
{
  public class MultipartFormDataSerializer
  {
    protected ObjectSerializer Serializer;


    public MultipartFormDataSerializer(Type t)
    {
      Serializer = new ObjectSerializer(t);
    }


    public void Serialize(TextWriter w, object data, string boundary = null)
    {
      MultipartFormDataPropertyVisitor v = new MultipartFormDataPropertyVisitor(w, boundary);
      Serializer.Serialize(data, v);
    }
  }
}
