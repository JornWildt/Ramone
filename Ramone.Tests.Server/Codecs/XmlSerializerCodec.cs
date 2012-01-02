using System.Xml;
using System.Xml.Serialization;


namespace Ramone.Tests.Server.Codecs
{
  public class XmlSerializerCodec<T> : XmlCodecBase<T>
    where T : class
  {
    XmlSerializer Serializer = new XmlSerializer(typeof(T));


    protected override void WriteTo(T d, XmlWriter writer)
    {
      Serializer.Serialize(writer, d);
    }


    protected override T ReadFrom(XmlReader reader)
    {
      return (T)Serializer.Deserialize(reader);
    }
  }
}