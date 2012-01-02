using System.ServiceModel.Syndication;
using System.Xml;
using OpenRasta.Codecs;


namespace Ramone.Tests.Server.Codecs
{
  [MediaType("application/atom+xml;q=0.9", "atom")]
  [MediaType("application/xml;q=0.9", "atom")]
  public class AtomItemCodec : XmlCodecBase<SyndicationItem>
  {
    protected override void WriteTo(SyndicationItem item, XmlWriter writer)
    {
      item.SaveAsAtom10(writer);
    }


    protected override SyndicationItem ReadFrom(XmlReader reader)
    {
     return SyndicationItem.Load(reader);
    }
  }
}