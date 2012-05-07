using System.Xml;
using System.Xml.Serialization;
using OpenRasta.Codecs;
using Ramone.Tests.Common.CMS;
using Ramone.Tests.Server.Codecs;


namespace Ramone.Tests.Server.CMS.Codecs
{
  [MediaType(CMSConstants.CMSMediaTypeId)]
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