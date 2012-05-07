using System.Xml;
using System.Xml.Serialization;
using OpenRasta.Codecs;
using Ramone.Tests.Common.CMS;
using Ramone.Tests.Server.Codecs;


namespace Ramone.Tests.Server.CMS.Codecs
{
  [MediaType(CMSConstants.CMSMediaTypeId)]
  [MediaType("application/xml")]
  public class DocumentCodec : XmlCodecBase<Document>
  {
    XmlSerializer Serializer = new XmlSerializer(typeof(Document));


    protected override void WriteTo(Document p, XmlWriter writer)
    {
      Serializer.Serialize(writer, p);
    }


    protected override Document ReadFrom(XmlReader reader)
    {
      return (Document)Serializer.Deserialize(reader);
    }
  }
}