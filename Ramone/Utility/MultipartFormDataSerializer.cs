using System;
using System.IO;
using System.Web;
using System.Text;


namespace Ramone.Utility
{
  public class MultipartFormDataSerializer
  {
    protected ObjectSerializer Serializer;


    public MultipartFormDataSerializer(Type t)
    {
      Serializer = new ObjectSerializer(t);
    }


    public void Serialize(Stream s, object data, Encoding encoding = null, string boundary = null)
    {
      MultipartFormDataPropertyVisitor v = new MultipartFormDataPropertyVisitor(s, encoding, boundary);
      v.Begin();
      Serializer.Serialize(data, v);
      v.End();
    }
  }
}
