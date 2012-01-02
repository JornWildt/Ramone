using System.ServiceModel.Syndication;
using System.Xml;
using System.IO;
using Ramone.MediaTypes.Xml;


namespace Ramone.MediaTypes.Atom
{
  public class AtomFeedCodec : XmlCodecBase<SyndicationFeed>
  {
    protected override SyndicationFeed ReadFrom(XmlReader reader)
    {
      return SyndicationFeed.Load(reader);
    }

    protected override void WriteTo(SyndicationFeed feed, XmlWriter writer)
    {
      feed.SaveAsAtom10(writer);
    }
  }
}
