using System.ServiceModel.Syndication;
using System.Xml;
using Ramone.MediaTypes.Xml;


namespace Ramone.MediaTypes.Atom
{
  public class AtomItemCodec : XmlStreamCodecBase
  {
    protected override object ReadFrom(XmlReader reader, ReaderContext context)
    {
      return SyndicationItem.Load(reader);
    }

    protected override void WriteTo(object item, XmlWriter writer, WriterContext context)
    {
      SyndicationItem si = (SyndicationItem)item;
      si.SaveAsAtom10(writer);
    }
  }
}
