using System.Xml;
using System.Xml.Serialization;


namespace Ramone.MediaTypes.Xml
{
  public class XmlSerializerCodec<TEntity> : XmlCodecBase<TEntity> 
    where TEntity : class
  {
    protected XmlSerializer Serializer { get; set; }


    public XmlSerializerCodec()
    {
      Serializer = CreateSerializer();
    }


    protected override TEntity ReadFrom(XmlReader reader)
    {
      return (TEntity)Serializer.Deserialize(reader);
    }


    protected override void WriteTo(TEntity entity, XmlWriter writer)
    {
      Serializer.Serialize(writer, entity);
    }


    protected virtual XmlSerializer CreateSerializer()
    {
      return new XmlSerializer(typeof(TEntity));
    }
  }
}
