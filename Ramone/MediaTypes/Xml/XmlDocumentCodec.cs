using System.Xml;


namespace Ramone.MediaTypes.Xml
{
  public class XmlDocumentCodec : XmlCodecBase<XmlDocument>
  {
    protected override XmlDocument ReadFrom(XmlReader reader)
    {
      XmlDocument doc = new XmlDocument();
      doc.Load(reader);
      return doc;
    }


    protected override void WriteTo(XmlDocument doc, XmlWriter writer)
    {
      doc.Save(writer);
    }
  }
}
