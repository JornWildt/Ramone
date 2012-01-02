using System.ServiceModel.Syndication;
using System.Xml;
using OpenRasta.Codecs;


namespace Ramone.Tests.Server.Codecs
{
  [MediaType("application/atom+xml;q=0.9", "atom")]
  [MediaType("application/atom;q=0.9", "atom")]
  public class AtomFeedCodec : XmlCodecBase<SyndicationFeed>
  {
    protected override void WriteTo(SyndicationFeed item, XmlWriter writer)
    {
      item.SaveAsAtom10(writer);
    }


    protected override SyndicationFeed ReadFrom(XmlReader reader)
    {
     return SyndicationFeed.Load(reader);
    }
  }
}