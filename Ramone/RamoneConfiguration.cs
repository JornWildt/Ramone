using System;
using System.ServiceModel.Syndication;
using System.Xml;
using HtmlAgilityPack;
using Ramone.Implementation;
using Ramone.MediaTypes.Atom;
using Ramone.MediaTypes.Html;
using Ramone.MediaTypes.Json;
using Ramone.MediaTypes.Xml;


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
      settings.CodecManager.AddCodec<HtmlDocument>("application/html+xml", new HtmlDocumentCodec());
      settings.CodecManager.AddCodec<HtmlDocument>("text/html", new HtmlDocumentCodec());
      settings.CodecManager.AddCodec<SyndicationFeed>("application/atom+xml", new AtomFeedCodec());
      settings.CodecManager.AddCodec<SyndicationItem>("application/atom+xml", new AtomItemCodec());
      settings.CodecManager.AddCodec("application/json", new JsonDynamicCodec());
      return settings;
    }
  }
}
