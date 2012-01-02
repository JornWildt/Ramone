using System.ServiceModel.Syndication;
using System.Xml;
using Ramone.MediaTypes.Xml;


namespace Ramone.MediaTypes.Atom
{
  public class AtomItemCodec : XmlCodecBase<SyndicationItem>
  {
    protected override SyndicationItem ReadFrom(XmlReader reader)
    {
      return SyndicationItem.Load(reader);
    }

    protected override void WriteTo(SyndicationItem item, XmlWriter writer)
    {
      item.SaveAsAtom10(writer);
    }
  }
}
