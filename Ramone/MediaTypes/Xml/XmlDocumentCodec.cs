using System.Xml;


namespace Ramone.MediaTypes.Xml
{
  public class XmlDocumentCodec : XmlStreamCodecBase
  {
    protected override object ReadFrom(XmlReader reader, ReaderContext context)
    {
      XmlDocument doc = new XmlDocument();
      doc.Load(reader);
      return doc;
    }


    protected override void WriteTo(object item, XmlWriter writer, WriterContext context)
    {
      XmlDocument doc = (XmlDocument)item;
      doc.Save(writer);
    }
  }
}
