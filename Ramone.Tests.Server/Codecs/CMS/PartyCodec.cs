using System.Xml;
using System.Xml.Serialization;
using OpenRasta.Codecs;
using Ramone.Tests.Common.CMS;


namespace Ramone.Tests.Server.Codecs.CMS
{
  [MediaType(CMSConstants.CMSMediaType)]
  [MediaType("application/xml")]
  public class PartyCodec : XmlCodecBase<Party>
  {
    XmlSerializer Serializer = new XmlSerializer(typeof(Party));


    protected override void WriteTo(Party p, XmlWriter writer)
    {
      Serializer.Serialize(writer, p);
    }


    protected override Party ReadFrom(XmlReader reader)
    {
      return (Party)Serializer.Deserialize(reader);
    }
  }
}