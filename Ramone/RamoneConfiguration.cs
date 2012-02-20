using System;
using System.IO;
using System.ServiceModel.Syndication;
using System.Text;
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

    public static Encoding DefaultEncoding { get; set; }

    public static ObjectSerializerSettings SerializerSettings { get; set; }


    static RamoneConfiguration()
    {
      Reset();
    }


    public static void Reset()
    {
      UseStandardCodecs = true;
      UserAgent = "Ramone/1.0";
      DefaultEncoding = Encoding.UTF8;
      SerializerSettings = new ObjectSerializerSettings();
    }

    
    public static IRamoneService NewService(Uri baseUrl)
    {
      IRamoneService service = new RamoneService(baseUrl)
      {
        UserAgent = UserAgent,
        DefaultEncoding = DefaultEncoding,
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
      cm.AddCodec(MediaType.ApplicationXml, new XmlSerializerCodec());
      cm.AddCodec<XmlDocument>(MediaType.ApplicationXml, new XmlDocumentCodec());
      cm.AddCodec<XmlDocument>(MediaType.TextXml, new XmlDocumentCodec());

      // HTML + XHTML
      cm.AddCodec<HtmlDocument>(MediaType.TextHtml, new HtmlDocumentCodec());
      cm.AddCodec<HtmlDocument>(MediaType.TextXml, new HtmlDocumentCodec());
      cm.AddCodec<HtmlDocument>(MediaType.ApplicationXHtml, new HtmlDocumentCodec());
      cm.AddCodec<HtmlDocument>(MediaType.ApplicationXml, new HtmlDocumentCodec());

      // Atom
      cm.AddCodec<SyndicationFeed>(MediaType.ApplicationAtom, new AtomFeedCodec());
      cm.AddCodec<SyndicationItem>(MediaType.ApplicationAtom, new AtomItemCodec());

      // JSON
      cm.AddCodec(MediaType.ApplicationJson, new JsonSerializerCodec());

      // Multipart/form-data
      cm.AddCodec(MediaType.MultipartFormData, new MultipartFormDataSerializerCodec());

      // Form url encoded
      cm.AddCodec(MediaType.ApplicationFormUrlEncoded, new FormUrlEncodedSerializerCodec());

      // Strings
      cm.AddCodec<String>(new MediaType("*/*"), new StringCodec());

      // Streams
      cm.AddCodec<Stream>(new MediaType("*/*"), new StreamCodec());
    }


    public static void RegisterStandardSerializationFormaters(IObjectSerializerFormaterManager formatManager)
    {
      formatManager.AddFormater(typeof(Uri), new UriFormater());
    }
  }
}
