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
using Ramone.Utility.ObjectSerialization;
using Ramone.Utility.ObjectSerialization.Formaters;


namespace Ramone
{
  public static class RamoneConfiguration
  {
    public static bool UseStandardCodecs { get; set; }

    public static string UserAgent { get; set; }

    public static ObjectSerializerSettings SerializerSettings { get; set; }


    static RamoneConfiguration()
    {
      Reset();
    }


    public static void Reset()
    {
      UseStandardCodecs = true;
      UserAgent = "Ramone/1.0";
      SerializerSettings = new ObjectSerializerSettings();
    }

    
    public static IRamoneService NewService(Uri baseUrl)
    {
      IRamoneService service = new RamoneService(baseUrl)
      {
        UserAgent = UserAgent,
        SerializerSettings = new ObjectSerializerSettings(SerializerSettings)
      };
      if (UseStandardCodecs)
        RamoneConfiguration.RegisterStandardCodecs(service.CodecManager);
      return service;
    }


    /// <summary>
    /// Create a new session with implicit service.
    /// </summary>
    /// <remarks>This will create a new session just as if it was created normally from a service. The only difference is
    /// that a new implicit service is create automatically on each call to NewSession(). This method is primarily for "getting
    /// started" scenarios. Any serious usage of Ramone should create and configure a specific service.</remarks>
    /// <param name="baseUrl">Base URL for implicit service.</param>
    /// <returns>A new session.</returns>
    public static IRamoneSession NewSession(Uri baseUrl)
    {
      IRamoneService service = NewService(baseUrl);
      return service.NewSession();
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


    public static void RegisterStandardSerializationFormaters(IObjectSerializerFormaterManager formatManager)
    {
      formatManager.AddFormater(typeof(Uri), new UriFormater());
    }
  }
}
