using System;
using System.IO;
using System.ServiceModel.Syndication;
using System.Xml;
using HtmlAgilityPack;
using Ramone.Implementation;
using Ramone.MediaTypes;
using Ramone.MediaTypes.Atom;
using Ramone.MediaTypes.FormUrlEncoded;
using Ramone.MediaTypes.Html;
using Ramone.MediaTypes.Json;
using Ramone.MediaTypes.MultipartFormData;
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
      RegisterStandardCodecs(settings.CodecManager);
      return settings;
    }


    public static void RegisterStandardCodecs(ICodecManager cm)
    {
      // XML
      cm.AddCodec("application/xml", new XmlSerializerCodec());
      cm.AddCodec<XmlDocument>("application/xml", new XmlDocumentCodec());
      cm.AddCodec<XmlDocument>("text/xml", new XmlDocumentCodec());

      // HTML + XHTML
      cm.AddCodec<HtmlDocument>("text/html", new HtmlDocumentCodec());
      cm.AddCodec<HtmlDocument>("text/xml", new HtmlDocumentCodec());
      cm.AddCodec<HtmlDocument>("application/xhtml+xml", new HtmlDocumentCodec());
      cm.AddCodec<HtmlDocument>("application/xml", new HtmlDocumentCodec());

      // Atom
      cm.AddCodec<SyndicationFeed>("application/atom+xml", new AtomFeedCodec());
      cm.AddCodec<SyndicationItem>("application/atom+xml", new AtomItemCodec());

      // JSON
      cm.AddCodec("application/json", new JsonSerializerCodec());

      // Multipart/form-data
      cm.AddCodec("multipart/form-data", new MultipartFormDataSerializerCodec());

      // Form url encoded
      cm.AddCodec("application/x-www-form-urlencoded", new FormUrlEncodedSerializerCodec());

      // Streams
      cm.AddCodec<Stream>(new StreamCodec());
    }
  }
}
