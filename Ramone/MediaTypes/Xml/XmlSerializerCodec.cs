using System.Xml;
using System.Xml.Serialization;
using System;
using System.Collections.Generic;


namespace Ramone.MediaTypes.Xml
{
  public class XmlSerializerCodec : XmlStreamCodecBase
  {
    // The XmlSerializer is thread safe according to the online docs, so it should be safe
    // to share instances.
    static Dictionary<Type, XmlSerializer> Serializers { get; set; }


    static XmlSerializerCodec()
    {
      Serializers = new Dictionary<Type, XmlSerializer>();
    }


    protected override object ReadFrom(XmlReader reader, ReaderContext context)
    {
      XmlSerializer serializer = GetSerializer(context.DataType);
      return serializer.Deserialize(reader);
    }


    protected override void WriteTo(object item, XmlWriter writer, WriterContext context)
    {
      if (item == null)
        throw new ArgumentNullException("item");

      XmlSerializer serializer = GetSerializer(item.GetType());
      serializer.Serialize(writer, item);
    }


    protected XmlSerializer GetSerializer(Type t)
    {
      if (!Serializers.ContainsKey(t))
      {
        Serializers[t] = CreateSerializer(t);
      }
      return Serializers[t];
    }


    protected virtual XmlSerializer CreateSerializer(Type t)
    {
      return new XmlSerializer(t);
    }
  }
}
