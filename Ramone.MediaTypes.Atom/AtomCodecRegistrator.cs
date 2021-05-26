using System.ServiceModel.Syndication;

namespace Ramone.MediaTypes.Atom
{
  public class AtomCodecRegistrator : ICodecRegistrator
  {
    public void RegisterCodecs(ICodecManager cm)
    {
      cm.AddCodec<SyndicationFeed, AtomFeedCodec>(MediaType.ApplicationAtom);
      cm.AddCodec<SyndicationItem, AtomItemCodec>(MediaType.ApplicationAtom);
    }
  }
}
