using System.ServiceModel.Syndication;

namespace Ramone.MediaTypes.Atom
{
  class AtomInitializer
  {
    void RegisterStandardCodecs(ICodecManager cm)
    {
      cm.AddCodec<SyndicationFeed, AtomFeedCodec>(MediaType.ApplicationAtom);
      cm.AddCodec<SyndicationItem, AtomItemCodec>(MediaType.ApplicationAtom);
    }
  }
}
