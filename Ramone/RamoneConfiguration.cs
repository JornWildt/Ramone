using System.ServiceModel.Syndication;
using System.Xml;
using Ramone.Implementation;
using Ramone.MediaTypes.Hal;
using Ramone.MediaTypes.Xml;
using Ramone.MediaTypes.Atom;
using System;


namespace Ramone
{
  public static class RamoneConfiguration
  {
    public static IRamoneService NewService(Uri baseUrl)
    {
      return new RamoneService(baseUrl);
    }


    public static IRamoneService WithStandardCodecs(this IRamoneService settings)
    {
      settings.CodecManager.AddCodec<XmlDocument>("application/xml", new XmlDocumentCodec());
      settings.CodecManager.AddCodec<SyndicationFeed>("application/atom+xml", new AtomFeedCodec());
      settings.CodecManager.AddCodec<SyndicationItem>("application/atom+xml", new AtomItemCodec());
      settings.CodecManager.AddCodec<HalResource>("application/hal+xml", new AtomItemCodec());
      return settings;
    }
  }
}
