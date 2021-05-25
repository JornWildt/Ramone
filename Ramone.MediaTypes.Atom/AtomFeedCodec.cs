using System.ServiceModel.Syndication;
using System.Xml;
using System.IO;
using Ramone.MediaTypes.Xml;


namespace Ramone.MediaTypes.Atom
{
  public class AtomFeedCodec : XmlStreamCodecBase
  {
    protected override object ReadFrom(XmlReader reader, ReaderContext context)
    {
      return SyndicationFeed.Load(reader);
    }

    protected override void WriteTo(object item, XmlWriter writer, WriterContext context)
    {
      SyndicationFeed feed = (SyndicationFeed)item;
      feed.SaveAsAtom10(writer);
    }
  }
}
